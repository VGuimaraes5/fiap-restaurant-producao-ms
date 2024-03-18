using Application.Models.ValueObject;
using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Application.Models.PedidoModel
{
    public class PedidoPostRequest
    {
        public string Id { get; set; }
        public List<ProdutoVO> Produtos { get; set; }
        public TipoPagamento TipoPagamento { get; set; }
        public Guid? IdCliente { get; set; }
        public string Senha { get; set; }
    }
}
