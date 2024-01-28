using System.Threading.Tasks;
using AutoMapper;
using Domain.Gateways;
using Domain.Entities;
using Application.Models.PedidoModel;
using System;
using Domain.Services;

namespace Application.UseCases.PedidoUseCase
{
    public class PostPedidoUseCaseAsync : IUseCaseAsync<PedidoPostRequest, Tuple<int, string>>
    {
        private readonly IPedidoGateway _pedidoGateway;
        private readonly IPagamentoGateway _pagamentoGateway;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public PostPedidoUseCaseAsync(
            IPedidoGateway pedidoGateway,
            IPagamentoGateway pagamentoGateway,
            IMapper mapper,
            IIdentityService identityService)
        {
            _pedidoGateway = pedidoGateway;
            _pagamentoGateway = pagamentoGateway;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<Tuple<int, string>> ExecuteAsync(PedidoPostRequest request)
        {
            if (request.Produtos == null || request.Produtos.Count == 0)
                throw new ArgumentException("Dados do pedido são inválidos");

            var pedido = new Pedido();
            pedido.SetSenha(_pedidoGateway.GetSequence());

            foreach (var item in request.Produtos)
            {
                var itemPedido = new ItemPedido(item.NomeProduto, item.ValorProduto, item.Observacao);
                pedido.AddItemPedido(itemPedido);
            }

            var userId = _identityService.GetUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                pedido.IdCliente = Guid.Parse(userId);
            }

            pedido = await _pedidoGateway.AddAsync(pedido);

            await _pagamentoGateway.CreateAsync(request.Pagamento.Tipo, pedido.Id);

            return new Tuple<int, string>(pedido.Senha, pedido.Id);
        }
    }
}
