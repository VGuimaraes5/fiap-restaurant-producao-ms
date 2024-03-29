using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.PedidoModel;
using Application.UseCases;
using Domain.Enums;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics.CodeAnalysis;

namespace Application.Consumers
{
    public class PagamentoErrorConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IUseCaseAsync<PedidoAlteraStatusRequest> _alteraStatusUseCase;
        private readonly IUseCaseAsync<PedidoAlteraStatusPagamentoRequest> _alteraStatusPagamentoUseCase;

        public PagamentoErrorConsumer(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            var scope = serviceScopeFactory.CreateScope();
            _configuration = configuration;
            _alteraStatusUseCase = scope.ServiceProvider.GetRequiredService<IUseCaseAsync<PedidoAlteraStatusRequest>>();
            _alteraStatusPagamentoUseCase = scope.ServiceProvider.GetRequiredService<IUseCaseAsync<PedidoAlteraStatusPagamentoRequest>>();
        }

        [ExcludeFromCodeCoverage]
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connectionFactory = new ConnectionFactory { Uri = new Uri(_configuration.GetConnectionString("RabbitMQ") ?? throw new InvalidOperationException("Invalid RabbitMQ connection string!")) };
            var exchange = _configuration["Exchange:PaymentError"] ?? throw new InvalidOperationException("Exchange not found!");

            using var connection = connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Fanout);
            var queueName = channel.QueueDeclare().QueueName;
            var consumer = new EventingBasicConsumer(channel);

            channel.QueueBind(queueName, exchange, "");
            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            consumer.Received += this.Consume;

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        public async void Consume(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var data = JsonSerializer.Deserialize<PagamentoStatusModel>(message);

                await _alteraStatusUseCase.ExecuteAsync(new PedidoAlteraStatusRequest
                {
                    PedidoId = data.PedidoId,
                    Status = (short)Status.Reprovado
                });

                await _alteraStatusPagamentoUseCase.ExecuteAsync(new PedidoAlteraStatusPagamentoRequest
                {
                    PedidoId = data.PedidoId,
                    Status = (short)StatusPagamento.Reprovado
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}