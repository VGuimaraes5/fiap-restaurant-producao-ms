using Application.Models.ValueObject;
using Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Application.Models.PedidoModel
{
    public class PedidoPostRequest
    {
        [JsonProperty("produtos")]
        public List<ProdutoDto> Produtos { get; set; }

        [JsonProperty("Pagamento")]
        public PagamentoDto Pagamento { get; set; }

    }
}
