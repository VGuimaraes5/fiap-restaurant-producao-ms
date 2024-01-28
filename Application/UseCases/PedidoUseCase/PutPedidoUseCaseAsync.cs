using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Application.Models.PedidoModel;
using Domain.Gateways;

namespace Application.UseCases.PedidoUseCase
{
    public class PutPedidoUseCaseAsync : IUseCaseAsync<PedidoPutRequest>
    {
        private readonly IPedidoGateway _pedidoGateway;
        private readonly IPagamentoGateway _pagamentoGateway;

        public PutPedidoUseCaseAsync(
            IPedidoGateway pedidoGateway,
            IPagamentoGateway pagamentoGateway)
        {
            _pedidoGateway = pedidoGateway;
            _pagamentoGateway = pagamentoGateway;
        }

        public async Task ExecuteAsync(PedidoPutRequest request)
        {
            var pedido = await _pedidoGateway.GetAsync(request.Id);
            var status = await _pagamentoGateway.GetStatusAsync(request.Id);

            if (string.IsNullOrEmpty(status))
                throw new KeyNotFoundException("Pedido não encontrado");
            if (status == GetDescriptionFromEnumValue(Domain.Enums.StatusPagamento.Pendente))
                throw new KeyNotFoundException("Pagamento pendente");
            if (status == GetDescriptionFromEnumValue(Domain.Enums.StatusPagamento.Reprovado))
                throw new KeyNotFoundException("Pagamento reprovado");

            pedido.SetStatus(request.Status);
            await _pedidoGateway.UpdateAsync(pedido);
        }

        public string GetDescriptionFromEnumValue(Enum value)
        {
            DescriptionAttribute attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}