using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Application.Models.PedidoModel;
using Domain.Gateways;

namespace Application.UseCases.PedidoUseCase
{
    public class AlteraStatusPedidoUseCaseAsync : IUseCaseAsync<PedidoAlteraStatusRequest>
    {
        private readonly IPedidoGateway _pedidoGateway;

        public AlteraStatusPedidoUseCaseAsync(IPedidoGateway pedidoGateway)
        {
            _pedidoGateway = pedidoGateway;
        }

        public async Task ExecuteAsync(PedidoAlteraStatusRequest request)
        {
            var pedido = await _pedidoGateway.GetByPedidoIdAsync(request.PedidoId);

            if (pedido == null)
                throw new KeyNotFoundException("Pedido não encontrado");

            pedido.SetStatus(request.Status);
            await _pedidoGateway.UpdateStatusAsync(pedido);
        }
    }
}