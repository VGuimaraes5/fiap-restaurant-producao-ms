﻿using System.Threading.Tasks;
using AutoMapper;
using Application.Models.ClienteModel;
using Domain.Gateways;

namespace Application.UseCases.ClienteUseCase
{
    public class GetClienteByCPFUseCaseAsync : IUseCaseAsync<ClienteRequest, ClienteResponse>
    {
        private readonly IClienteGateway _clienteGateway;
        private readonly IMapper _mapper;

        public GetClienteByCPFUseCaseAsync(IClienteGateway clienteGateway, IMapper mapper)
        {
            _clienteGateway = clienteGateway;
            _mapper = mapper;
        }

        public async Task<ClienteResponse> ExecuteAsync(ClienteRequest request)
        {
            var result = await _clienteGateway.GetByCPFAsync(request.Cpf);

            if (result == null)
                return null;

            return new ClienteResponse
            {
                Id = result.Id,
                Cpf = result.Cpf.ToString(),
                Nome = result.Nome
            };
        }
    }
}
