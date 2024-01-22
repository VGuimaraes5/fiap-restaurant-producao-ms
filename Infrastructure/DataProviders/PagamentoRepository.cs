using System.Text;
using Domain.Domain;
using Domain.Enums;
using Domain.Gateways;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Infrastructure.DataProviders
{
    public class PagamentoRepository : IPagamentoGateway
    {
        private readonly HttpClient _client;

        public PagamentoRepository(IConfiguration configuration)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(configuration["URL:Pagamento"]);
        }

        public async Task CreateAsync(TipoPagamento tipo, string pedidoId)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(new
            {
                tipo,
                pedidoId
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("Pagamento", content);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Erro ao cadastrar o pagamento");
        }

        public async Task<string> GetStatusAsync(string pedidoId)
        {
            var response = await _client.GetAsync($"Pagamento/StatusPagamento/{pedidoId}");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<PagamentoStatusResult>(jsonResponse);
                return result?.Status ?? "";
            }

            return "";
        }
    }
}