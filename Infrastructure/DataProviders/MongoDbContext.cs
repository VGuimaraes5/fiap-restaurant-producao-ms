using Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Infrastructure.DataProviders
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var mongoClient = new MongoClient(configuration["Mongo:ConnectionString"]);
            _database = mongoClient.GetDatabase(configuration["Mongo:Database"]);
        }

        public IMongoCollection<Pedido> Pedido
        {
            get
            {
                return _database.GetCollection<Pedido>("Pedido");
            }
        }
    }
}