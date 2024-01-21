using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Models.PagamentoModel;
using Application.Utils;
using Domain.Gateways.External;

namespace Application.UseCases.PagamentoUseCase
{
    public class GetPagamentoUseCaseAsync : IUseCaseAsync<PagamentoGetRequest, Tuple<string, Guid>>
    {
        private readonly IPagamentoGateway _pagamentoGateway;

        public GetPagamentoUseCaseAsync(IPagamentoGateway pagamentoGateway)
        {
            _pagamentoGateway = pagamentoGateway;
        }

        public async Task<Tuple<string, Guid>> ExecuteAsync(PagamentoGetRequest request)
        {
            var pagamento = await _pagamentoGateway.GetByPedidoAsync(request.IdPedido);
            if (pagamento == null)
                throw new KeyNotFoundException("Pagamento não encontrado");

            return new Tuple<string, Guid>(
                EnumUtil.GetDescriptionFromEnumValue(pagamento.Status),
                pagamento.Id
            );
        }
    }
}