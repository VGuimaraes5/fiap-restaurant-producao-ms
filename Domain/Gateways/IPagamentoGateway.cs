using System;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Gateways
{
    public interface IPagamentoGateway
    {
        Task CreateAsync(TipoPagamento Tipo, string pedidoId);
        Task<string> GetStatusAsync(string pedidoId);
    }
}