using Domain.Entities.Base;
using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Pedido : Entity
    {
        private Pedido() { }

        public Pedido(Guid? id = null, Status? status = null, Guid? idCliente = null)
        {
            this.Status = status == null ? Status.Pendente : (Status)status;
            this.IdCliente = idCliente;
            ValidateEntity();
        }

        public int Senha { get; private set; }
        public Status Status { get; private set; }
        public Guid? IdCliente { get; set; }
        public List<ItemPedido> ItensPedido { get; set; }

        public void AddItemPedido(ItemPedido itemPedido)
        {
            if (ItensPedido == null) ItensPedido = new List<ItemPedido>();
            ItensPedido.Add(itemPedido);
        }

        public void SetStatus(short status)
        {
            this.Status = (Status)status;
            ValidateEntity();
        }

        public void SetSenha(int senha)
        {
            this.Senha = senha;
        }

        private void ValidateEntity()
        {
            AssertionConcern.AssertArgumentNotNull(this.Status, "O Status não pode estar vazio!");
            AssertionConcern.AssertArgumentRange((short)this.Status, 0, 3, "O Status informado não existe");
        }
    }
}
