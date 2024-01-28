using Application.Models.PedidoModel;
using Application.UseCases.PedidoUseCase;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Gateways;
using Moq;


namespace Test.Application.UseCases.PedidoUseCase
{
    public class PutPedidoUseCaseAsyncTests
    {
        [Fact]
        public async Task ExecuteAsync_UpdatesPedidoStatus_WhenStatusIsValid()
        {
            // Arrange
            var mockPedidoGateway = new Mock<IPedidoGateway>();
            var mockPagamentoGateway = new Mock<IPagamentoGateway>();
            var useCase = new PutPedidoUseCaseAsync(mockPedidoGateway.Object, mockPagamentoGateway.Object);
            var request = new PedidoPutRequest { Id = "pedido01", Status = (short)Status.Pendente };

            mockPedidoGateway.Setup(m => m.GetAsync(It.IsAny<string>())).ReturnsAsync(new Pedido());
            mockPagamentoGateway.Setup(m => m.GetStatusAsync(It.IsAny<string>())).ReturnsAsync("OK");

            // Act
            await useCase.ExecuteAsync(request);

            // Assert
            mockPedidoGateway.Verify(m => m.UpdateAsync(It.IsAny<Pedido>()), Times.Once);
        }
        [Fact]
        public async Task ExecuteAsync_ThrowsException_WhenStatusIsNull()
        {
            // Arrange
            var mockPedidoGateway = new Mock<IPedidoGateway>();
            var mockPagamentoGateway = new Mock<IPagamentoGateway>();
            var useCase = new PutPedidoUseCaseAsync(mockPedidoGateway.Object, mockPagamentoGateway.Object);
            var request = new PedidoPutRequest { Id = "pedido01", Status = (short)Status.Pendente };

            mockPedidoGateway.Setup(m => m.GetAsync(It.IsAny<string>())).ReturnsAsync(new Pedido());
            mockPagamentoGateway.Setup(m => m.GetStatusAsync(It.IsAny<string>())).ReturnsAsync(string.Empty);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => useCase.ExecuteAsync(request));
        }

        [Fact]
        public async Task ExecuteAsync_ThrowsException_WhenStatusIsPending()
        {
            // Arrange
            var mockPedidoGateway = new Mock<IPedidoGateway>();
            var mockPagamentoGateway = new Mock<IPagamentoGateway>();
            var useCase = new PutPedidoUseCaseAsync(mockPedidoGateway.Object, mockPagamentoGateway.Object);
            var request = new PedidoPutRequest { Id = "pedido01", Status = (short)Status.Pendente };
        
            mockPedidoGateway.Setup(m => m.GetAsync(It.IsAny<string>())).ReturnsAsync(new Pedido());
            mockPagamentoGateway.Setup(m => m.GetStatusAsync(It.IsAny<string>())).ReturnsAsync(useCase.GetDescriptionFromEnumValue(StatusPagamento.Pendente));
        
            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => useCase.ExecuteAsync(request));
        }
        
        [Fact]
        public async Task ExecuteAsync_ThrowsException_WhenStatusIsRejected()
        {
            // Arrange
            var mockPedidoGateway = new Mock<IPedidoGateway>();
            var mockPagamentoGateway = new Mock<IPagamentoGateway>();
            var useCase = new PutPedidoUseCaseAsync(mockPedidoGateway.Object, mockPagamentoGateway.Object);
            var request = new PedidoPutRequest { Id = "pedido01", Status = (short)Status.Pendente };
        
            mockPedidoGateway.Setup(m => m.GetAsync(It.IsAny<string>())).ReturnsAsync(new Pedido());
            mockPagamentoGateway.Setup(m => m.GetStatusAsync(It.IsAny<string>())).ReturnsAsync(useCase.GetDescriptionFromEnumValue(StatusPagamento.Reprovado));
        
            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => useCase.ExecuteAsync(request));
        }
    }

}