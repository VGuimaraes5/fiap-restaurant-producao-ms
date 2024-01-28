using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Models.PedidoModel;
using Application.UseCases;
using Moq;
using Microsoft.Extensions.Logging;
using API.Controllers;

namespace Test.API.Controllers
{
    public class PedidoControllerTests
    {
        private readonly Mock<IUseCaseIEnumerableAsync<PedidoRequest, PedidoDetalhadoPorSenhaResponse>> _getBySenhaCase;
        private readonly Mock<IUseCaseIEnumerableAsync<IEnumerable<PedidoDetalhadoResponse>>> _getAlluseCase;
        private readonly Mock<IUseCaseIEnumerableAsync<IEnumerable<HistoricoClienteResponse>>> _getHistoricoClienteUseCase;
        private readonly Mock<IUseCaseAsync<PedidoPostRequest, Tuple<int, string>>> _postUseCase;
        private readonly Mock<IUseCaseAsync<PedidoPutRequest>> _putUseCase;
        private readonly Mock<IUseCaseAsync<PedidoDeleteRequest>> _deleteUseCase;
        private readonly Mock<ILogger<PedidoController>> _logger;
        private readonly PedidoController _controller;

        public PedidoControllerTests()
        {
            _getBySenhaCase = new Mock<IUseCaseIEnumerableAsync<PedidoRequest, PedidoDetalhadoPorSenhaResponse>>();
            _getAlluseCase = new Mock<IUseCaseIEnumerableAsync<IEnumerable<PedidoDetalhadoResponse>>>();
            _getHistoricoClienteUseCase = new Mock<IUseCaseIEnumerableAsync<IEnumerable<HistoricoClienteResponse>>>();
            _postUseCase = new Mock<IUseCaseAsync<PedidoPostRequest, Tuple<int, string>>>();
            _putUseCase = new Mock<IUseCaseAsync<PedidoPutRequest>>();
            _deleteUseCase = new Mock<IUseCaseAsync<PedidoDeleteRequest>>();
            _logger = new Mock<ILogger<PedidoController>>();
            _controller = new PedidoController(_logger.Object, _getBySenhaCase.Object, _getAlluseCase.Object, _postUseCase.Object, _putUseCase.Object, _deleteUseCase.Object, _getHistoricoClienteUseCase.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnOkResult_WhenPedidosExist()
        {
            // Arrange
            var pedidos = new List<PedidoDetalhadoResponse> { new PedidoDetalhadoResponse() };
            _getAlluseCase.Setup(x => x.ExecuteAsync()).ReturnsAsync(pedidos);

            // Act
            var result = await _controller.GetAllAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<PedidoDetalhadoResponse>>(okResult.Value);
            Assert.Equal(pedidos, returnValue);
        }

        [Fact]
        public async Task GetPedidoBySenha_ShouldReturnOk_WhenPedidoExists()
        {
            // Arrange
            var pedido = new PedidoDetalhadoPorSenhaResponse();
            var request = new PedidoRequest { Senha = 1001 };
            _getBySenhaCase.Setup(x => x.ExecuteAsync(It.IsAny<PedidoRequest>())).ReturnsAsync(pedido);

            // Act
            var result = await _controller.GetPedidoBySenha(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PedidoDetalhadoPorSenhaResponse>(okResult.Value);
            Assert.Equal(pedido, returnValue);
        }

        [Fact]
        public async Task GetHistoricoCliente_ShouldReturnOk_WhenHistoricoExists()
        {
            // Arrange
            var historico = new List<HistoricoClienteResponse> { new HistoricoClienteResponse() };
            _getHistoricoClienteUseCase.Setup(x => x.ExecuteAsync()).ReturnsAsync(historico);

            // Act
            var result = await _controller.GetHistoricoCliente();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<HistoricoClienteResponse>>(okResult.Value);
            Assert.Equal(historico, returnValue);
        }

        [Fact]
        public async Task GetHistoricoCliente_ShouldReturnOkWithEmptyList_WhenHistoricoDoesNotExist()
        {
            // Arrange
            _getHistoricoClienteUseCase.Setup(x => x.ExecuteAsync()).ReturnsAsync(new List<HistoricoClienteResponse>());

            // Act
            var result = await _controller.GetHistoricoCliente();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<HistoricoClienteResponse>>(okResult.Value);
            Assert.Empty(returnValue);
        }

        [Fact]
        public async Task Put_ShouldReturnOk_WhenPedidoIsValid()
        {
            // Arrange
            var request = new PedidoPutRequest();
            _putUseCase.Setup(x => x.ExecuteAsync(It.IsAny<PedidoPutRequest>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Put("1", request);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Put_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var request = new PedidoPutRequest();
            _putUseCase.Setup(x => x.ExecuteAsync(It.IsAny<PedidoPutRequest>())).Throws(new Exception());

            // Act
            var result = await _controller.Put("1", request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenPedidoIsValid()
        {
            // Arrange
            var request = new PedidoDeleteRequest();
            _deleteUseCase.Setup(x => x.ExecuteAsync(It.IsAny<PedidoDeleteRequest>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(request);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var request = new PedidoDeleteRequest();
            _deleteUseCase.Setup(x => x.ExecuteAsync(It.IsAny<PedidoDeleteRequest>())).Throws(new Exception());

            // Act
            var result = await _controller.Delete(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }





    }

}