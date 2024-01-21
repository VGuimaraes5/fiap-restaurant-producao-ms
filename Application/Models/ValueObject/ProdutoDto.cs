﻿using Newtonsoft.Json;
using System;

namespace Application.Models.ValueObject
{
    public class ProdutoDto
    {
        [JsonProperty("produtoId")]
        public Guid ProdutoId { get; set; }

        [JsonProperty("observacao")]
        public string Observacao { get; set; }
    }
}
