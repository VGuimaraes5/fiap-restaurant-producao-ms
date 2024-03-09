namespace Application.Models.PedidoModel
{
    public class PedidoAlteraStatusPagamentoRequest
    {
        public string PedidoId { get; set; }
        public short Status { get; set; }
    }
}