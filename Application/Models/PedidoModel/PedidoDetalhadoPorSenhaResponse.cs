﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Application.Models.PedidoModel
{
    public class PedidoDetalhadoPorSenhaResponse
    {
        [JsonProperty("Senha")]
        public string Senha { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("StatusPagamento")]
        public string StatusPagamento { get; set; }

        [JsonProperty("Total")]
        public decimal Total { get; set; }

        [JsonProperty("PedidoId")]
        public string PedidoId { get; set; }

        [JsonProperty("ItensPedido")]
        public List<PedidoProdutoDetalhadoPorSenhaResponse> ItensPedido { get; set; }
    }

    public class PedidoProdutoDetalhadoPorSenhaResponse
    {
        [JsonProperty("NomeProduto")]
        public string NomeProduto { get; set; }

        [JsonProperty("NomeCategoria")]
        public string NomeCategoria { get; set; }

        [JsonProperty("Valor")]
        public decimal Valor { get; set; }

        [JsonProperty("Observacao")]
        public string Observacao { get; set; }
    }
}
