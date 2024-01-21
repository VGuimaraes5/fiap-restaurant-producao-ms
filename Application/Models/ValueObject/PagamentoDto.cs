﻿using Domain.Enums;
using Newtonsoft.Json;

namespace Application.Models.ValueObject
{
    public class PagamentoDto
    {
        [JsonProperty("tipo")]
        public TipoPagamento Tipo { get; set; }
    }
}
