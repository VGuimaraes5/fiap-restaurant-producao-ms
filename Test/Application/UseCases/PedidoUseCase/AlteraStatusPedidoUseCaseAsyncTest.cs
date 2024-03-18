using Application.Models.PedidoModel;
using Application.UseCases.PedidoUseCase;
using Domain.Entities;
using Domain.Enums;
using Domain.Gateways;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Test.Application.UseCases.PedidoUseCase
{
    public class AlteraStatusPedidoUseCaseAsyncTests
    {
        [Fact]
        public async Task ExecuteAsync_ThrowsKeyNotFoundException_WhenPedidoNotFound()
        {
            // Arrange
            var mockPedidoGateway = new Mock<IPedidoGateway>();
            var useCase = new AlteraStatusPedidoUseCaseAsync(mockPedidoGateway.Object);
            var request = new PedidoAlteraStatusRequest { PedidoId = "pedido-01" };
            mockPedidoGateway.Setup(gateway => gateway.GetByPedidoIdAsync(request.PedidoId)).ReturnsAsync((Pedido?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => useCase.ExecuteAsync(request));
            Assert.Equal("Pedido não encontrado", exception.Message);
        }

        [Fact]
        public async Task ExecuteAsync_SetsStatusAndCallsUpdateStatusAsync_WhenPedidoFound()
        {
            // Arrange
            var mockPedidoGateway = new Mock<IPedidoGateway>();
            var useCase = new AlteraStatusPedidoUseCaseAsync(mockPedidoGateway.Object);
            var request = new PedidoAlteraStatusRequest { PedidoId = "pedido-01", Status = (short)Status.EmPreparo };
            var pedido = new Pedido(status: Status.Pendente);
            mockPedidoGateway.Setup(gateway => gateway.GetByPedidoIdAsync(request.PedidoId)).ReturnsAsync(pedido);

            // Act
            await useCase.ExecuteAsync(request);

            // Assert
            Assert.Equal(Status.EmPreparo, pedido.Status);
            mockPedidoGateway.Verify(gateway => gateway.UpdateStatusAsync(pedido), Times.Once);
        }
    }
}