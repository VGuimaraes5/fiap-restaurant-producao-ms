using System;

namespace Application.Models.PedidoModel
{
    public class PedidoAlteraStatusRequest
    {
        public string PedidoId { get; set; }
        public short Status { get; set; }
    }
}