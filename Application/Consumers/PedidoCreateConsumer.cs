using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.PedidoModel;
using Application.Models.ValueObject;
using Application.UseCases;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics.CodeAnalysis;

namespace Application.Consumers
{
    public class PedidoCreateConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IUseCaseAsync<PedidoPostRequest> _postUseCase;

        public PedidoCreateConsumer(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            var scope = serviceScopeFactory.CreateScope();
            _configuration = configuration;
            _postUseCase = scope.ServiceProvider.GetRequiredService<IUseCaseAsync<PedidoPostRequest>>();
        }

        [ExcludeFromCodeCoverage]
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connectionFactory = new ConnectionFactory { Uri = new Uri(_configuration.GetConnectionString("RabbitMQ") ?? throw new InvalidOperationException("Invalid RabbitMQ connection string!")) };
            var exchange = _configuration["Exchange:PedidoCreate"] ?? throw new InvalidOperationException("Exchange not found!");

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
                var data = JsonSerializer.Deserialize<PedidoModel>(message);

                await _postUseCase.ExecuteAsync(new PedidoPostRequest
                {
                    Id = data.PedidoId,
                    IdCliente = data.IdCliente,
                    Senha = data.Senha,
                    TipoPagamento = data.TipoPagamento,
                    Produtos = data.Produtos.Select(item => new ProdutoVO
                    {
                        NomeProduto = item.NomeProduto,
                        Observacao = item.Observacao,
                        ValorProduto = item.ValorProduto
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}