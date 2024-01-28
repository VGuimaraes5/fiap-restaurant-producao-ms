using System;
using Domain.Entities.Base;

namespace Domain.Entities
{
    public class ItemPedido
    {
        private ItemPedido() { }

        public ItemPedido(string nomeProduto, decimal valorProduto, string observacao)
        {
            this.NomeProduto = nomeProduto;
            this.ValorProduto = valorProduto;
            this.Observacao = observacao;

            ValidateEntity();
        }

        public string NomeProduto { get; set; }
        public decimal ValorProduto { get; set; }
        public string Observacao { get; set; }

        public virtual Pedido Pedido { get; set; }

        private void ValidateEntity()
        {
            AssertionConcern.AssertArgumentNotNull(NomeProduto, "O nome do produto não pode estar vazio!");
            AssertionConcern.AssertArgumentLength(NomeProduto, 200, "A nome do produto deve conter no máximo 500 caracteres");
            AssertionConcern.AssertArgumentNotNull(ValorProduto, "O valor do produto não pode estar vazio!");
            if (!string.IsNullOrEmpty(Observacao))
                AssertionConcern.AssertArgumentLength(Observacao, 500, "A observação deve conter no máximo 500 caracteres");
        }
    }
}