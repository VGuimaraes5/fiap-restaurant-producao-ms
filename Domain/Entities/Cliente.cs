﻿using System;
using Domain.Entities.Base;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Cliente : Entity<Guid>
    {
        private Cliente() { }

        public Cliente(string nome, string cpf, string userId, Guid? id = null)
        {
            Id = id == null ? Guid.NewGuid() : (Guid)id;
            Nome = nome;
            Cpf = cpf;
            UserId = userId;
            ValidateEntity();
        }

        public string Nome { get; private set; }
        public Cpf Cpf { get; private set; }
        public string UserId { get; set; }

        public void CheckClienteAlreadyExists(Cliente cliente)
        {
            if (cliente == null) return;
            AssertionConcern.AssertArgumentNotEquals(cliente.Cpf, Cpf, "Já existe um cliente com este CPF cadastrado!");
        }

        private void ValidateEntity()
        {
            AssertionConcern.AssertArgumentNotNull(Id, "O Id não pode estar vazio!");
            AssertionConcern.AssertArgumentNotEmpty(Nome, "O nome não pode estar vazio!");
            AssertionConcern.AssertArgumentNotEmpty(UserId, "O userId não pode estar vazio!");
        }
    }
}
