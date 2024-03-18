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
        [Fact]
        public async Task ExecuteAsync_ThrowsArgumentException_WhenProdutosIsNull()
        {
            // Arrange
            var mockPedidoGateway = new Mock<IPedidoGateway>();
            var mockIdentityService = new Mock<IIdentityService>();
            var mockMapper = new Mock<IMapper>();
            var useCase = new PostPedidoUseCaseAsync(mockPedidoGateway.Object, mockMapper.Object, mockIdentityService.Object);
            var request = new PedidoPostRequest { Produtos = null };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(request));
            Assert.Equal("Dados do pedido são inválidos", exception.Message);
        }

        [Fact]
        public async Task ExecuteAsync_ThrowsArgumentException_WhenProdutosIsEmpty()
        {
            // Arrange
            var mockPedidoGateway = new Mock<IPedidoGateway>();
            var mockIdentityService = new Mock<IIdentityService>();
            var mockMapper = new Mock<IMapper>();
            var useCase = new PostPedidoUseCaseAsync(mockPedidoGateway.Object, mockMapper.Object, mockIdentityService.Object);
            var request = new PedidoPostRequest { Produtos = new List<ProdutoVO>() };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(request));
            Assert.Equal("Dados do pedido são inválidos", exception.Message);
        }

        [Fact]
        public async Task ExecuteAsync_CreatesItemPedidoForEachProductInRequest()
        {
            // Arrange
            var mockPedidoGateway = new Mock<IPedidoGateway>();
            var mockIdentityService = new Mock<IIdentityService>();
            var mockMapper = new Mock<IMapper>();
            var useCase = new PostPedidoUseCaseAsync(mockPedidoGateway.Object, mockMapper.Object, mockIdentityService.Object);
            var request = new PedidoPostRequest
            {
                Produtos = new List<ProdutoVO>
            {
                new ProdutoVO { NomeProduto = "Lanche", ValorProduto = 10.50M, Observacao = "s/ cebola" },
                new ProdutoVO { NomeProduto = "Suco", ValorProduto = 8.70M, Observacao = "s/ açucar" }
            }
            };

            // Act
            await useCase.ExecuteAsync(request);

            // Assert
            mockPedidoGateway.Verify(gateway => gateway.AddAsync(It.Is<Pedido>(pedido =>
                pedido.ItensPedido.Count == 2 &&
                pedido.ItensPedido.Any(item => item.NomeProduto == "Lanche" && item.ValorProduto == 10.50M && item.Observacao == "s/ cebola") &&
                pedido.ItensPedido.Any(item => item.NomeProduto == "Suco" && item.ValorProduto == 8.70M && item.Observacao == "s/ açucar")
            )), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_SetsIdCliente_WhenGetUserIdReturnsNonEmptyString()
        {
            // Arrange
            var mockPedidoGateway = new Mock<IPedidoGateway>();
            var mockIdentityService = new Mock<IIdentityService>();
            var mockMapper = new Mock<IMapper>();
            var useCase = new PostPedidoUseCaseAsync(mockPedidoGateway.Object, mockMapper.Object, mockIdentityService.Object);
            var request = new PedidoPostRequest
            {
                Produtos = new List<ProdutoVO>
            {
                new ProdutoVO { NomeProduto = "Lanche", ValorProduto = 10.50M, Observacao = "completo" }
            }
            };
            var userId = Guid.NewGuid().ToString();
            mockIdentityService.Setup(service => service.GetUserId()).Returns(userId);

            // Act
            await useCase.ExecuteAsync(request);

            // Assert
            mockPedidoGateway.Verify(gateway => gateway.AddAsync(It.Is<Pedido>(pedido =>
                pedido.IdCliente == Guid.Parse(userId)
            )), Times.Once);
        }
    }
}