﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Application.Enums;
using Application.Models.ProdutoModel;
using Domain.Entities;
using Domain.Gateways;

namespace Application.UseCases.ProdutoUseCase
{
    public class GetProdutoByCategoriaIdUseCaseAsync : IUseCaseIEnumerableAsync<ProdutoRequest, IEnumerable<ProdutoResponse>>
    {
        private readonly IProdutoGateway _gateway;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public GetProdutoByCategoriaIdUseCaseAsync(IProdutoGateway gateway, IMapper mapper, IMemoryCache memoryCache)
        {
            _gateway = gateway;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<ProdutoResponse>> ExecuteAsync(ProdutoRequest request)
        {
            var key = CacheKeys.CategoriaProduto + request.CategoriaId;

            if (!_memoryCache.TryGetValue(key, out IEnumerable<Produto> cacheValue))
            {
                var result = await _gateway.GetProdutoByCategoriaIdAsync(request.CategoriaId);
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(5));
                _memoryCache.Set(key, result, cacheEntryOptions);

                return _mapper.Map<IEnumerable<ProdutoResponse>>(result);
            }

            return _mapper.Map<IEnumerable<ProdutoResponse>>(cacheValue);
        }
    }
}
