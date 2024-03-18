using Application.Models.PedidoModel;
using Application.Models.ValueObject;
using Domain.Enums;
using System.Collections.Generic;
using Xunit;


namespace Test.Application.Models.PedidoModel
{
    public class PedidoPostRequestTests
    {
        [Fact]
        public void TestPedidoPostRequestProperties()
        {
            var model = new PedidoPostRequest
            {
                Produtos = new List<ProdutoVO>
            {
                new ProdutoVO
                {
                    NomeProduto = "Lanche01",
                    ValorProduto = 50.25m,
                    Observacao = "n/a"
                }
            },
               
            };

            Assert.NotNull(model.Produtos);
            Assert.Single(model.Produtos);
            Assert.Equal("Lanche01", model.Produtos[0].NomeProduto);
            Assert.Equal(50.25m, model.Produtos[0].ValorProduto);
            Assert.Equal("n/a", model.Produtos[0].Observacao);
        }
    }
}