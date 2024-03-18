using System;
using System.Collections.Generic;
using Domain.Enums;

namespace Domain.Models
{
    public class PedidoModel
    {
        public string PedidoId { get; set; }
        public List<PedidoProdutoModel> Produtos { get; set; }
        public TipoPagamento TipoPagamento { get; set; }
        public Guid? IdCliente { get; set; }
        public string Senha { get; set; }
    }
}