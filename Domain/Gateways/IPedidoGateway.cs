using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Gateways
{
    public interface IPedidoGateway
    {
        Task<Pedido> GetAsync(string id);
        Task<Pedido> GetByPedidoIdAsync(string id);
        Task DeleteAsync(string id);
        Task<Pedido> GetPedidoBySenhaUseCaseAsync(string senha);
        Task<IEnumerable<Pedido>> GetPedidosDetalhadosAsync();
        Task<Pedido> AddAsync(Pedido pedido);
        Task UpdateStatusAsync(Pedido pedido);
        Task UpdateStatusPagamentoAsync(Pedido pedido);
    }
}
