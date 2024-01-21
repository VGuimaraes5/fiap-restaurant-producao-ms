﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Application.Models.PedidoModel;
using Application.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly IUseCaseIEnumerableAsync<PedidoRequest, PedidoDetalhadoPorSenhaResponse> _getBySenhaCase;
        private readonly IUseCaseIEnumerableAsync<IEnumerable<PedidoDetalhadoResponse>> _getAlluseCase;
        private readonly IUseCaseIEnumerableAsync<IEnumerable<HistoricoClienteResponse>> _getHistoricoClienteUseCase;
        private readonly IUseCaseAsync<PedidoPostRequest, Tuple<int, Guid>> _postUseCase;
        private readonly IUseCaseAsync<PedidoPutRequest> _putUseCase;
        private readonly IUseCaseAsync<PedidoDeleteRequest> _deleteUseCase;
        private readonly ILogger<PedidoController> _logger;

        public PedidoController(ILogger<PedidoController> logger,
            IUseCaseIEnumerableAsync<PedidoRequest, PedidoDetalhadoPorSenhaResponse> getBySenhauseCase,
            IUseCaseIEnumerableAsync<IEnumerable<PedidoDetalhadoResponse>> getAlluseCase,
            IUseCaseAsync<PedidoPostRequest, Tuple<int, Guid>> postUseCase,
            IUseCaseAsync<PedidoPutRequest> putUseCase,
            IUseCaseAsync<PedidoDeleteRequest> deleteUseCase,
            IUseCaseIEnumerableAsync<IEnumerable<HistoricoClienteResponse>> getHistoricoClienteUseCase)
        {
            _logger = logger;
            _getBySenhaCase = getBySenhauseCase;
            _getAlluseCase = getAlluseCase;
            _postUseCase = postUseCase;
            _putUseCase = putUseCase;
            _deleteUseCase = deleteUseCase;
            _getHistoricoClienteUseCase = getHistoricoClienteUseCase;
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

        [Authorize]
        [HttpGet("historico")]
        public async Task<IActionResult> GetHistoricoCliente()
        {
            try
            {
                var result = await _getHistoricoClienteUseCase.ExecuteAsync();

                if (result != null)
                    return Ok(result);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Checkout")]
        public async Task<IActionResult> Post([FromBodyAttribute] PedidoPostRequest request)
        {
            try
            {
                var senhaPedido = await _postUseCase.ExecuteAsync(request);
                
                return Ok(new
                {
                    Senha = senhaPedido.Item1,
                    PedidoId = senhaPedido.Item2
                });
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPut("AtualizarStatus/{Id}")]
        public async Task<IActionResult> Put([FromRoute] Guid Id, [FromBodyAttribute] PedidoPutRequest request)
        {
            try
            {
                request.Id = Id;
                await _putUseCase.ExecuteAsync(request);
                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] PedidoDeleteRequest request)
        {
            try
            {
                await _deleteUseCase.ExecuteAsync(request);
                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}
