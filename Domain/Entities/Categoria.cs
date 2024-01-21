﻿using System;
using System.Collections.Generic;
using Domain.Entities.Base;

namespace Domain.Entities
{
    public class Categoria : Entity<Guid>
    {
        private Categoria() { }

        public Categoria(string nome, Guid? id = null)
        {
            Id = id == null ? Guid.NewGuid() : (Guid)id;
            Nome = nome;
            ValidateEntity();
        }

        public string Nome { get; private set; }

        public List<Produto> Produtos { get; set; }

        private void ValidateEntity()
        {
            AssertionConcern.AssertArgumentNotNull(Id, "O Id não pode estar vazio!");
            AssertionConcern.AssertArgumentNotEmpty(Nome, "O nome não pode estar vazio!");
        }
    }
}
