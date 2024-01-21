﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Application.Enums;
using Application.Models.PedidoModel;
using Domain.Entities;
using Domain.Gateways;

namespace Application.UseCases.PedidoUseCase
{
    public class GetAllPedidoUseCaseAsync : IUseCaseIEnumerableAsync<IEnumerable<PedidoDetalhadoResponse>>
    {
        private readonly IPedidoGateway _gateway;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public GetAllPedidoUseCaseAsync(IPedidoGateway gateway, IMapper mapper, IMemoryCache memoryCache)
        {
            _gateway = gateway;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<PedidoDetalhadoResponse>> ExecuteAsync()
        {
            if (!_memoryCache.TryGetValue(CacheKeys.TodosPedidos, out IEnumerable<Pedido> cacheValue))
            {
                var result = await _gateway.GetPedidosDetalhadosAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(10));
                _memoryCache.Set(CacheKeys.TodosPedidos, result, cacheEntryOptions);

                return _mapper.Map<IEnumerable<PedidoDetalhadoResponse>>(result);
            }

            return _mapper.Map<IEnumerable<PedidoDetalhadoResponse>>(cacheValue);
        }
    }
}
