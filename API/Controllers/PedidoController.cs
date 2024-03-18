using Microsoft.AspNetCore.Mvc;
using Application.Models.PedidoModel;
using Application.UseCases;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly IUseCaseIEnumerableAsync<PedidoRequest, PedidoDetalhadoPorSenhaResponse> _getBySenhaCase;
        private readonly IUseCaseIEnumerableAsync<IEnumerable<PedidoDetalhadoResponse>> _getAlluseCase;
        private readonly IUseCaseAsync<PedidoAlteraStatusRequest> _alteraStatusUseCase;

        public PedidoController(
            IUseCaseIEnumerableAsync<PedidoRequest, PedidoDetalhadoPorSenhaResponse> getBySenhauseCase,
            IUseCaseIEnumerableAsync<IEnumerable<PedidoDetalhadoResponse>> getAlluseCase,
            IUseCaseAsync<PedidoAlteraStatusRequest> alteraStatusUseCase)
        {
            _getBySenhaCase = getBySenhauseCase;
            _getAlluseCase = getAlluseCase;
            _alteraStatusUseCase = alteraStatusUseCase;
        }

        [HttpGet("Acompanhamento")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await _getAlluseCase.ExecuteAsync();
                if (result.Any())
                    return Ok(result);

                return NoContent();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("{Senha}")]
        public async Task<IActionResult> GetPedidoBySenha([FromRoute] PedidoRequest request)
        {
            try
            {
                var result = await _getBySenhaCase.ExecuteAsync(request);
                if (result != null)
                    return Ok(result);

                return NoContent();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPut("AtualizarStatus/{Id}")]
        public async Task<IActionResult> Put([FromRoute] string Id, [FromBody] PedidoAlteraStatusRequest request)
        {
            try
            {
                request.PedidoId = Id;
                await _alteraStatusUseCase.ExecuteAsync(request);
                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}
