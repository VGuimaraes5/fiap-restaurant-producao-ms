using Application.Models.PedidoModel;
using Xunit;

namespace Test.Application.Models.PedidoModel
{
    public class HistoricoClienteResponseTests
    {
        [Fact]
        public void TestHistoricoClienteResponseProperties()
        {
            var model = new HistoricoClienteResponse
            {
                Senha = 1234,
                Total = 100.50m,
                ItensPedido = new List<HistoricoClienteProdutoUseCaseResponse>()
            };

            Assert.Equal(1234, model.Senha);
            Assert.Equal(100.50m, model.Total);
            Assert.NotNull(model.ItensPedido);
        }
    }

    public class HistoricoClienteProdutoUseCaseResponseTests
    {
        [Fact]
        public void TestHistoricoClienteProdutoUseCaseResponseProperties()
        {
            var model = new HistoricoClienteProdutoUseCaseResponse
            {
                NomeProduto = "Lanche01",
                NomeCategoria = "Lanche",
                Valor = 50.25m,
                Observacao = "n/a"
            };

            Assert.Equal("Lanche01", model.NomeProduto);
            Assert.Equal("Lanche", model.NomeCategoria);
            Assert.Equal(50.25m, model.Valor);
            Assert.Equal("n/a", model.Observacao);
        }
    }
}