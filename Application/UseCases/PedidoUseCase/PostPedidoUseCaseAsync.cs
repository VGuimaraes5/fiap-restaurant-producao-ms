using System.Threading.Tasks;
using AutoMapper;
using Domain.Gateways;
using Domain.Entities;
using Application.Models.PedidoModel;
using System;
using Domain.Services;

namespace Application.UseCases.PedidoUseCase
{
    public class PostPedidoUseCaseAsync : IUseCaseAsync<PedidoPostRequest>
    {
        private readonly IPedidoGateway _pedidoGateway;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public PostPedidoUseCaseAsync(
            IPedidoGateway pedidoGateway,
            IMapper mapper,
            IIdentityService identityService)
        {
            _pedidoGateway = pedidoGateway;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task ExecuteAsync(PedidoPostRequest request)
        {
            if (request.Produtos == null || request.Produtos.Count == 0)
                throw new ArgumentException("Dados do pedido são inválidos");

            var pedido = new Pedido();
            pedido.PedidoId = request.Id;
            pedido.SetSenha(request.Senha);

            foreach (var item in request.Produtos)
            {
                var itemPedido = new ItemPedido(item.NomeProduto, item.ValorProduto, item.Observacao);
                pedido.AddItemPedido(itemPedido);
            }

            var userId = _identityService.GetUserId();
            if (!string.IsNullOrEmpty(userId))
                pedido.IdCliente = Guid.Parse(userId);

            await _pedidoGateway.AddAsync(pedido);
        }
    }
}
