using Application.Models.PedidoModel;
using Application.Models.ValueObject;
using Application.UseCases.PedidoUseCase;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Gateways;
using Domain.Services;
using Moq;

namespace Test.Application.UseCases.PedidoUseCase
{
    public class PostPedidoUseCaseAsyncTests
    {
        private readonly Mock<IPedidoGateway> _pedidoGateway;
        private readonly Mock<IPagamentoGateway> _pagamentoGateway;
        private readonly Mock<IIdentityService> _identityService;
        private readonly Mock<IMapper> _mapper;

        public PostPedidoUseCaseAsyncTests()
        {
            _pedidoGateway = new Mock<IPedidoGateway>();
            _pagamentoGateway = new Mock<IPagamentoGateway>();
            _identityService = new Mock<IIdentityService>();
            _mapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnPedido_WhenPedidoIsValid()
        {
            // Arrange
            var request = new PedidoPostRequest
            {
                Produtos = new List<ProdutoDto>
            {
                new ProdutoDto { NomeProduto = "Test", ValorProduto = 10, Observacao = "Test" }
            },
                Pagamento = new PagamentoDto { Tipo = TipoPagamento.Pix }
            };

            var pedido = new Pedido();
            _pedidoGateway.Setup(x => x.GetSequence()).Returns(1);
            _pedidoGateway.Setup(x => x.AddAsync(It.IsAny<Pedido>())).ReturnsAsync(pedido);

            var userId = Guid.NewGuid().ToString();
            _identityService.Setup(x => x.GetUserId()).Returns(userId);

            var useCase = new PostPedidoUseCaseAsync(_pedidoGateway.Object, _pagamentoGateway.Object, _mapper.Object, _identityService.Object);

            // Act
            var result = await useCase.ExecuteAsync(request);

            // Assert
            Assert.Equal(pedido.Senha, result.Item1);
            Assert.Equal(pedido.Id, result.Item2);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldThrowArgumentException_WhenPedidoIsInvalid()
        {
            // Arrange
            var request = new PedidoPostRequest { Produtos = new List<ProdutoDto>() };

            var useCase = new PostPedidoUseCaseAsync(_pedidoGateway.Object, _pagamentoGateway.Object, _mapper.Object, _identityService.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(request));
        }
    }
}