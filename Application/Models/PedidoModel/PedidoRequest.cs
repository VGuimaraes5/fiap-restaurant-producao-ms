using Microsoft.AspNetCore.Mvc;

namespace Application.Models.PedidoModel
{
    public class PedidoRequest
    {
        public PedidoRequest()
        {
            Senha = "";
        }

        [FromRoute]
        public string Senha { get; set; }
    }
}
