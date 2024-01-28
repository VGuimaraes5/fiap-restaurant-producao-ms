using System;

namespace Domain.Domain
{
    public class PagamentoStatusResult
    {
        public string Status { get; set; }
        public Guid PagamentoId { get; set; }
    }
}