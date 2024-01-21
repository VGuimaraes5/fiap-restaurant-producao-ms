using System;
using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities
{
    public class Pagamento : Entity<Guid>
    {
        private Pagamento() { }

        public Pagamento(TipoPagamento tipoPagamento, Guid pedidoId, StatusPagamento? status = null, Guid? id = null)
        {
            Id = id == null ? Guid.NewGuid() : (Guid)id;
            Status = status == null ? StatusPagamento.Pendente : (StatusPagamento)status;
            TipoPagamento = tipoPagamento;
            PedidoId = pedidoId;
            ValidateEntity();
        }

        public TipoPagamento TipoPagamento { get; private set; }
        public StatusPagamento Status { get; private set; }
        public Guid PedidoId { get; private set; }
        public virtual Pedido Pedido { get; set; }

        public void SetStatus(short status)
        {
            this.Status = (StatusPagamento)status;
            ValidateEntity();
        }

        private void ValidateEntity()
        {
            AssertionConcern.AssertArgumentNotNull(Id, "O Id não pode estar vazio!");
            AssertionConcern.AssertArgumentNotNull(this.Status, "O Status não pode estar vazio!");
            AssertionConcern.AssertArgumentRange((short)this.Status, 0, 2, "O Status informado não existe");
            AssertionConcern.AssertArgumentNotNull(this.TipoPagamento, "O Tipo Pagamento não pode estar vazio!");
            AssertionConcern.AssertArgumentRange((short)this.TipoPagamento, 0, 1, "O Tipo Pagamento informado não existe");
            AssertionConcern.AssertArgumentNotNull(this.PedidoId, "O id do pedido não pode estar vazio!");
        }
    }
}