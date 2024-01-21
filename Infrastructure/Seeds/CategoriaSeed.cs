using System;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Seeds
{
    public static class CategoriaSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria("Lanche", Guid.Parse("ada751db-8553-493f-b308-70bd29aed106")),
                new Categoria("Acompanhamento", Guid.Parse("cf412102-35da-43d8-9c3c-b72546104c72")),
                new Categoria("Bebida", Guid.Parse("5117243c-b007-49e8-9a30-842ec79248ae")),
                new Categoria("Sobremesa", Guid.Parse("32f0c5f0-d9ba-40e2-8d7a-57eed4727e2b"))
            );
        }
    }
}