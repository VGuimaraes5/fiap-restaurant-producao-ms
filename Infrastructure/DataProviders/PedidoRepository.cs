using Domain.Entities;
using Domain.Enums;
using Domain.Gateways;
using MongoDB.Driver;

namespace Infrastructure.DataProviders
{
    public class PedidoRepository : IPedidoGateway
    {
        private readonly MongoDbContext _context;

        public PedidoRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<Pedido> AddAsync(Pedido pedido)
        {
            await _context.Pedido.InsertOneAsync(pedido);
            return pedido;
        }

        public async Task DeleteAsync(string id)
        {
            await _context.Pedido.FindOneAndDeleteAsync(x => x.Id == id);
        }

        public async Task<Pedido> GetAsync(string id)
        {
            return await _context.Pedido.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Pedido> GetByPedidoIdAsync(string id)
        {
            return await _context.Pedido.Find(x => x.PedidoId == id).FirstOrDefaultAsync();
        }

        public async Task<Pedido> GetPedidoBySenhaUseCaseAsync(string senha)
        {
            return await _context.Pedido.Find(x => x.Senha == senha).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Pedido>> GetPedidosDetalhadosAsync()
        {
            return await _context.Pedido.Find(x => x.StatusPagamento == StatusPagamento.Aprovado && x.Status != Status.Entregue && x.Status != Status.Reprovado).ToListAsync();
        }

        public async Task UpdateStatusAsync(Pedido pedido)
        {
            await _context.Pedido.UpdateOneAsync(Builders<Pedido>.Filter.Where(x => x.Id == pedido.Id), Builders<Pedido>.Update.Set(p => p.Status, pedido.Status));
        }

        public async Task UpdateStatusPagamentoAsync(Pedido pedido)
        {
            await _context.Pedido.UpdateOneAsync(Builders<Pedido>.Filter.Where(x => x.Id == pedido.Id), Builders<Pedido>.Update.Set(p => p.StatusPagamento, pedido.StatusPagamento));
        }
    }
}