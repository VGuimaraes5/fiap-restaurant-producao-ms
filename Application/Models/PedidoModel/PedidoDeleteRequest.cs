using Microsoft.AspNetCore.Mvc;
using System;

namespace Application.Models.PedidoModel
{
    public class PedidoDeleteRequest
    {
        public PedidoDeleteRequest()
        {
            Id = "";
        }

        [FromRoute]
        public string Id { get; set; }
    }
}
