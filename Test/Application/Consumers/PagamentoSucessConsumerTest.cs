using Application.Consumers;
using Application.Models.PedidoModel;
using Application.UseCases;
using Domain.Enums;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Test.Application.Consumers
{
    public class PagamentoSuccessConsumerTests
    {
        [Fact]
        public void Consume_ProcessesMessageAndCallsExecuteAsync()
        {
            // Arrange
            var mockAlteraStatusUseCase = new Mock<IUseCaseAsync<PedidoAlteraStatusRequest>>();
            var mockAlteraStatusPagamentoUseCase = new Mock<IUseCaseAsync<PedidoAlteraStatusPagamentoRequest>>();
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(sp => sp.GetService(typeof(IUseCaseAsync<PedidoAlteraStatusRequest>))).Returns(mockAlteraStatusUseCase.Object);
            mockServiceProvider.Setup(sp => sp.GetService(typeof(IUseCaseAsync<PedidoAlteraStatusPagamentoRequest>))).Returns(mockAlteraStatusPagamentoUseCase.Object);
            var mockServiceScope = new Mock<IServiceScope>();
            mockServiceScope.Setup(ss => ss.ServiceProvider).Returns(mockServiceProvider.Object);
            var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
            mockServiceScopeFactory.Setup(ssf => ssf.CreateScope()).Returns(mockServiceScope.Object);
            var mockConfiguration = new Mock<IConfiguration>();
            var consumer = new PagamentoSuccessConsumer(mockServiceScopeFactory.Object, mockConfiguration.Object);
            var data = new PagamentoStatusModel { PedidoId = "pedido-01" };
            var message = JsonSerializer.Serialize(data);
            var body = Encoding.UTF8.GetBytes(message);
            var e = new BasicDeliverEventArgs { Body = body };

            // Act
            consumer.Consume(this, e);

            // Assert
            var expectedStatusRequest = new PedidoAlteraStatusRequest { PedidoId = data.PedidoId, Status = (short)Status.Pendente };
            var expectedStatusPagamentoRequest = new PedidoAlteraStatusPagamentoRequest { PedidoId = data.PedidoId, Status = (short)StatusPagamento.Aprovado };
            mockAlteraStatusUseCase.Verify(useCase => useCase.ExecuteAsync(It.Is<PedidoAlteraStatusRequest>(request => request.PedidoId == expectedStatusRequest.PedidoId && request.Status == expectedStatusRequest.Status)), Times.Once);
            mockAlteraStatusPagamentoUseCase.Verify(useCase => useCase.ExecuteAsync(It.Is<PedidoAlteraStatusPagamentoRequest>(request => request.PedidoId == expectedStatusPagamentoRequest.PedidoId && request.Status == expectedStatusPagamentoRequest.Status)), Times.Once);
        }
    }
}