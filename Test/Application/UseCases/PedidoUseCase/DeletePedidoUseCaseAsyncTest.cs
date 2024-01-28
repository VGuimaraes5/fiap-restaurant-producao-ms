using Application.Models.PedidoModel;
using Application.UseCases.PedidoUseCase;
using Domain.Entities;
using Domain.Gateways;
using Moq;

namespace Test.Application.UseCases.PedidoUseCase
{
    public class DeletePedidoUseCaseAsyncTest
    {
        private readonly Mock<IPedidoGateway> _gateway;

        public DeletePedidoUseCaseAsyncTest()
        {
            _gateway = new Mock<IPedidoGateway>();
        }

        [Fact]
        public async Task ExecuteAsync_WithValidRequest_ShouldDeletePedido()
        {
            // Arrange
            var request = new PedidoDeleteRequest { Id = "pedido01" };
            var pedido = new Pedido { Id = request.Id };
            _gateway.Setup(x => x.GetAsync(request.Id)).ReturnsAsync(pedido);
            var useCase = new DeletePedidoUseCaseAsync(_gateway.Object);

            // Act
            await useCase.ExecuteAsync(request);

            // Assert
            _gateway.Verify(x => x.DeleteAsync(request.Id), Times.Once);
        }
    }
}
