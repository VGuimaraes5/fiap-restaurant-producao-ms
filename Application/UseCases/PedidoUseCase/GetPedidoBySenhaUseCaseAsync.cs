﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Application.Models.PedidoModel;
using Domain.Entities;
using Domain.Gateways;

namespace Application.UseCases.PedidoUseCase
{
    public class GetPedidoBySenhaUseCaseAsync : IUseCaseIEnumerableAsync<PedidoRequest, PedidoDetalhadoPorSenhaResponse>
    {
        private readonly IPedidoGateway _gateway;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public GetPedidoBySenhaUseCaseAsync(IPedidoGateway gateway, IMapper mapper, IMemoryCache memoryCache)
        {
            _gateway = gateway;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task<PedidoDetalhadoPorSenhaResponse> ExecuteAsync(PedidoRequest request)
        {
            var key = request.Senha;

            if (!_memoryCache.TryGetValue(key, out IEnumerable<Pedido> cacheValue))
            {
                var result = await _gateway.GetPedidoBySenhaUseCaseAsync(request.Senha);
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(20));
                _memoryCache.Set(key, result, cacheEntryOptions);

                return _mapper.Map<PedidoDetalhadoPorSenhaResponse>(result);
            }

            return _mapper.Map<PedidoDetalhadoPorSenhaResponse>(cacheValue);
        }
    }
}
