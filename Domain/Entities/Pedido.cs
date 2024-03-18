using Domain.Entities.Base;
using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Pedido : Entity
    {
        private Pedido() { }

        public Pedido(Guid? id = null, Status? status = null, StatusPagamento? statusPagamento = null, Guid? idCliente = null)
        {
            this.Status = status == null ? Status.EmAnalise : (Status)status;
            this.StatusPagamento = statusPagamento == null ? StatusPagamento.Pendente : (StatusPagamento)statusPagamento;
            this.IdCliente = idCliente;
            ValidateEntity();
        }

        public string PedidoId { get; set; }
        public string Senha { get; private set; }
        public Status Status { get; private set; }
        public StatusPagamento StatusPagamento { get; private set; }
        public Guid? IdCliente { get; set; }
        public List<ItemPedido> ItensPedido { get; set; }

        public void AddItemPedido(ItemPedido itemPedido)
        {
            if (ItensPedido == null) ItensPedido = new List<ItemPedido>();
            ItensPedido.Add(itemPedido);
        }

        public void SetStatus(short status)
        {
            if (this.Status == Status.EmAnalise || this.Status == Status.Reprovado)
                throw new Exception("Não é possível atualizar o status!");

            this.Status = (Status)status;
            ValidateEntity();
        }

        public void SetStatusPagamento(short status)
        {
            if (this.StatusPagamento == StatusPagamento.Aprovado || this.StatusPagamento == StatusPagamento.Reprovado)
                throw new Exception("Não é possível atualizar o status!");

            this.StatusPagamento = (StatusPagamento)status;
            ValidateEntity();
        }

        public void SetSenha(string senha)
        {
            this.Senha = senha;
        }

        private void ValidateEntity()
        {
            AssertionConcern.AssertArgumentNotNull(this.Status, "O Status não pode estar vazio!");
            AssertionConcern.AssertArgumentRange((short)this.Status, 0, 3, "O Status informado não existe");
            AssertionConcern.AssertArgumentNotNull(this.StatusPagamento, "O Status de Pagamento não pode estar vazio!");
            AssertionConcern.AssertArgumentRange((short)this.StatusPagamento, 0, 2, "O Status de Pagamento informado não existe");
        }
    }
}
