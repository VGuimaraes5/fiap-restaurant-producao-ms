using Newtonsoft.Json;
using System;

namespace Application.Models.ValueObject
{
    public class ProdutoDto
    {
        [JsonProperty("nomeProduto")]
        public string NomeProduto { get; set; }

        [JsonProperty("valorProduto")]
        public decimal ValorProduto { get; set; }

        [JsonProperty("observacao")]
        public string Observacao { get; set; }
    }
}
