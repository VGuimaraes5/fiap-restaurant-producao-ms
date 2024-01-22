using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Gateways
{
    public interface IPedidoGateway
    {
        Task<Pedido> GetAsync(string id);
        Task DeleteAsync(string id);
        Task<Pedido> GetPedidoBySenhaUseCaseAsync(int senha);
        Task<IEnumerable<Pedido>> GetPedidosDetalhadosAsync();
        Task<IEnumerable<Pedido>> GetHistoricoAsync(string userId);
        Task<Pedido> AddAsync(Pedido pedido);
        Task UpdateAsync(Pedido pedido);
        int GetSequence();
    }
}
