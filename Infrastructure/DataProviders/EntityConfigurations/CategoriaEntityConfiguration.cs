﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.DataProviders.EntityConfigurations
{
    public class CategoriaEntityConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder
                .ToTable("tb_Categoria")
                .HasKey(p => new { p.Id });

            builder.Property(p => p.Nome)
               .HasColumnType("varchar(200)");

            builder.HasMany(p => p.Produtos)
                .WithOne(p => p.Categoria)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
