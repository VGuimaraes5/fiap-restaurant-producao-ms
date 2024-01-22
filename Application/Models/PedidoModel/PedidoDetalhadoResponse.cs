﻿using Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Application.Models.PedidoModel
{
    public class PedidoDetalhadoResponse
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Senha")]
        public int Senha { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }

        [JsonProperty("StatusPagamento")]
        public string StatusPagamento { get; set; }

        [JsonProperty("Total")]
        public decimal Total { get; set; }

        [JsonProperty("ItensPedido")]
        public List<PedidoProdutoDetalhadoResponse> ItensPedido { get; set; }
    }

    public class PedidoProdutoDetalhadoResponse
    {
        [JsonProperty("NomeProduto")]
        public string NomeProduto { get; set; }

        [JsonProperty("Valor")]
        public decimal Valor { get; set; }

        [JsonProperty("Observacao")]
        public string Observacao { get; set; }
    }
}
