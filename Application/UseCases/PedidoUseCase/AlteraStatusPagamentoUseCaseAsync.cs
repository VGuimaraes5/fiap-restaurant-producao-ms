using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Models.PedidoModel;
using Domain.Gateways;

namespace Application.UseCases.PedidoUseCase
{
    public class AlteraStatusPagamentoUseCaseAsync : IUseCaseAsync<PedidoAlteraStatusPagamentoRequest>
    {
        private readonly IPedidoGateway _pedidoGateway;

        public AlteraStatusPagamentoUseCaseAsync(IPedidoGateway pedidoGateway)
        {
            _pedidoGateway = pedidoGateway;
        }

        public async Task ExecuteAsync(PedidoAlteraStatusPagamentoRequest request)
        {
            var pedido = await _pedidoGateway.GetByPedidoIdAsync(request.PedidoId);

            if (pedido == null)
                throw new KeyNotFoundException("Pedido não encontrado");

            pedido.SetStatusPagamento(request.Status);
            await _pedidoGateway.UpdateStatusPagamentoAsync(pedido);
        }
    }
}