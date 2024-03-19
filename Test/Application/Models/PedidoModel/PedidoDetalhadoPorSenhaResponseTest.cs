using Application.Models.PedidoModel;
using Domain.Entities.Base;
using System.Collections.Generic;
using Xunit;

namespace Test.Application.Models.PedidoModel
{
    public class PedidoDetalhadoPorSenhaResponseTests
    {
        [Fact]
        public void TestPedidoDetalhadoPorSenhaResponseProperties()
        {
            var model = new PedidoDetalhadoPorSenhaResponse
            {
                Senha = "1234",
                Status = "Pendente",
                StatusPagamento = "Pendente",
                Total = 100.50m,
                PedidoId = "pedido-01",
                ItensPedido = new List<PedidoProdutoDetalhadoPorSenhaResponse>()
            };

            Assert.Equal("1234", model.Senha);
            Assert.Equal("Pendente", model.Status);
            Assert.Equal("Pendente", model.StatusPagamento);
            Assert.Equal(100.50m, model.Total);
            Assert.Equal("pedido-01", model.PedidoId);
            Assert.NotNull(model.ItensPedido);
        }
    }

    public class PedidoProdutoDetalhadoPorSenhaResponseTests
    {
        [Fact]
        public void TestPedidoProdutoDetalhadoPorSenhaResponseProperties()
        {
            var model = new PedidoProdutoDetalhadoPorSenhaResponse
            {
                NomeProduto = "Lanche01",
                NomeCategoria = "Lanche",
                Valor = 50.25m,
                Observacao = "msg teste"
            };

            Assert.Equal("Lanche01", model.NomeProduto);
            Assert.Equal("Lanche", model.NomeCategoria);
            Assert.Equal(50.25m, model.Valor);
            Assert.Equal("msg teste", model.Observacao);
        }
    }
}