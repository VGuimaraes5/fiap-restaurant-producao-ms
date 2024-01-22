using Microsoft.Extensions.DependencyInjection;
using Application.Models.PedidoModel;
using Application.UseCases;
using Application.UseCases.PedidoUseCase;
using Domain.Gateways;
using Infrastructure.DataProviders;
using Application.Services;
using Domain.Services;

namespace Infrastructure.Extensions
{
    public static class RegisterServiceExtensions
    {
        public static void RegisterService(this IServiceCollection services)
        {
            AddUseCase(services);
            AddRepositories(services);
            AddOthers(services);
        }

        private static void AddUseCase(IServiceCollection services)
        {
            services.AddTransient<IUseCaseIEnumerableAsync<IEnumerable<PedidoDetalhadoResponse>>, GetAllPedidoUseCaseAsync>();
            services.AddTransient<IUseCaseIEnumerableAsync<PedidoRequest, PedidoDetalhadoPorSenhaResponse>, GetPedidoBySenhaUseCaseAsync>();
            services.AddTransient<IUseCaseAsync<PedidoPostRequest, Tuple<int, string>>, PostPedidoUseCaseAsync>();
            services.AddTransient<IUseCaseAsync<PedidoDeleteRequest>, DeletePedidoUseCaseAsync>();
            services.AddTransient<IUseCaseAsync<PedidoPutRequest>, PutPedidoUseCaseAsync>();

            services.AddTransient<IUseCaseIEnumerableAsync<IEnumerable<HistoricoClienteResponse>>, GetHistoricoClienteUseCaseAsync>();

            //services.AddTransient<ICognitoGateway, CognitoService>();
            services.AddTransient<IIdentityService, IdentityService>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddTransient<IPedidoGateway, PedidoRepository>();
            services.AddTransient<IPagamentoGateway, PagamentoRepository>();
        }

        private static void AddOthers(IServiceCollection services)
        {
            services.AddScoped<MongoDbContext>();
        }
    }
}
