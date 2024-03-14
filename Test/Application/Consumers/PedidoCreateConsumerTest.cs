using Application.Consumers;
using Application.Models.PedidoModel;
using Application.Models.ValueObject;
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
    public class PedidoCreateConsumerTest
    {
        [Fact]
        public void Consume_ValidMessage_CallsPostUseCase()
        {
            // Arrange
            var serviceScopeFactory = new Mock<IServiceScopeFactory>();
            var configuration = new Mock<IConfiguration>();
            var postUseCase = new Mock<IUseCaseAsync<PedidoPostRequest>>();
            var serviceScope = new Mock<IServiceScope>();
            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider.Setup(x => x.GetService(typeof(IUseCaseAsync<PedidoPostRequest>))).Returns(postUseCase.Object);
            serviceScope.Setup(x => x.ServiceProvider).Returns(serviceProvider.Object);
            serviceScopeFactory.Setup(x => x.CreateScope()).Returns(serviceScope.Object);

            var consumer = new PedidoCreateConsumer(serviceScopeFactory.Object, configuration.Object);

            var pedidoModel = new PedidoModel
            {
                PedidoId = "pedido-01",
                IdCliente = Guid.NewGuid(),
                Senha = "password",
                TipoPagamento = TipoPagamento.Pix,
                Produtos = new List<PedidoProdutoModel>
            {
                new PedidoProdutoModel
                {
                    NomeProduto = "Lanche",
                    ValorProduto = 10.50M,
                    Observacao = "Capricha no bacon"
                }
            }
            };

            var message = JsonSerializer.Serialize(pedidoModel);
            var body = Encoding.UTF8.GetBytes(message);
            var eventArgs = new BasicDeliverEventArgs { Body = body };

            // Act
            consumer.Consume(this, eventArgs);

            // Assert
            postUseCase.Verify(x => x.ExecuteAsync(It.IsAny<PedidoPostRequest>()), Times.Once);
        }
    }
}