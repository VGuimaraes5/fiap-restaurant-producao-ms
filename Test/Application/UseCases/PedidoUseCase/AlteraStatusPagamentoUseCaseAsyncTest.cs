using Application.Models.PedidoModel;
using Application.UseCases.PedidoUseCase;
using Domain.Entities;
using Domain.Gateways;
using Domain.Enums;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Test.Application.UseCases.PedidoUseCase
{
    public class AlteraStatusPagamentoUseCaseAsyncTests
    {
        [Fact]
        public async Task ExecuteAsync_ThrowsKeyNotFoundException_WhenPedidoNotFound()
        {
            // Arrange
            var mockPedidoGateway = new Mock<IPedidoGateway>();
            var useCase = new AlteraStatusPagamentoUseCaseAsync(mockPedidoGateway.Object);
            var request = new PedidoAlteraStatusPagamentoRequest { PedidoId = "pedido-01" };
            mockPedidoGateway.Setup(gateway => gateway.GetByPedidoIdAsync(request.PedidoId)).ReturnsAsync((Pedido?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => useCase.ExecuteAsync(request));
            Assert.Equal("Pedido não encontrado", exception.Message);
        }

        [Fact]
        public async Task ExecuteAsync_SetsStatusAndCallsUpdateStatusPagamentoAsync_WhenPedidoFound()
        {
            // Arrange
            var mockPedidoGateway = new Mock<IPedidoGateway>();
            var useCase = new AlteraStatusPagamentoUseCaseAsync(mockPedidoGateway.Object);
            var request = new PedidoAlteraStatusPagamentoRequest { PedidoId = "pedido-01", Status = (short)StatusPagamento.Aprovado };
            var pedido = new Pedido();
            mockPedidoGateway.Setup(gateway => gateway.GetByPedidoIdAsync(request.PedidoId)).ReturnsAsync(pedido);

            // Act
            await useCase.ExecuteAsync(request);

            // Assert
            Assert.Equal(StatusPagamento.Aprovado, pedido.StatusPagamento);
            mockPedidoGateway.Verify(gateway => gateway.UpdateStatusPagamentoAsync(pedido), Times.Once);
        }
    }
}