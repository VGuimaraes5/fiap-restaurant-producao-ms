﻿using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.DataProviders.EntityConfigurations;
using Infrastructure.Seeds;

namespace Infrastructure.DataProviders
{
    public class DBContext : DbContext
    {
        public DbSet<Cliente> Cliente { get; set; }

        public DbSet<Categoria> Categoria { get; set; }

        public DbSet<Produto> Produto { get; set; }

        public DbSet<Pedido> Pedido { get; set; }

        public DbSet<ItemPedido> ItemPedido { get; set; }

        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ClienteEntityConfiguration());

            modelBuilder.ApplyConfiguration(new CategoriaEntityConfiguration());

            modelBuilder.ApplyConfiguration(new ProdutoEntityConfiguration());

            modelBuilder.ApplyConfiguration(new PedidoEntityConfiguration());

            modelBuilder.ApplyConfiguration(new ItemPedidoEntityConfiguration());

            modelBuilder.ApplyConfiguration(new PagamentoEntityConfiguration());

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbContext).Assembly);

            CategoriaSeed.Seed(modelBuilder);
            ProdutoSeed.Seed(modelBuilder);
        }
    }


}
