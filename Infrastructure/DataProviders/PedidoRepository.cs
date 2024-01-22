using Domain.Entities;
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

        public async Task<IEnumerable<Pedido>> GetHistoricoAsync(string userId)
        {
            return await _context.Pedido.Find(x => x.IdCliente == Guid.Parse(userId)).ToListAsync();
        }

        public async Task<Pedido> GetPedidoBySenhaUseCaseAsync(int senha)
        {
            return await _context.Pedido.Find(x => x.Senha == senha).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Pedido>> GetPedidosDetalhadosAsync()
        {
            return await _context.Pedido.Find(_ => true).ToListAsync();
        }

        public async Task UpdateAsync(Pedido pedido)
        {
            await _context.Pedido.UpdateOneAsync(Builders<Pedido>.Filter.Where(x => x.Id == pedido.Id), Builders<Pedido>.Update.Set(p => p.Status, pedido.Status));
        }

        public int GetSequence()
        {
            var pedido = _context.Pedido.AsQueryable().OrderByDescending(c => c.Senha).FirstOrDefault();
            if (pedido != null) return pedido.Senha + 1;

            return 1;
        }
    }
}