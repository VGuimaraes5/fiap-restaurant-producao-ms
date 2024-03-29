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
        private readonly Mock<IUseCaseAsync<PedidoAlteraStatusRequest>> _alteraStatusUseCase;
        private readonly Mock<ILogger<PedidoController>> _logger;
        private readonly PedidoController _controller;

        public PedidoControllerTests()
        {
            _getBySenhaCase = new Mock<IUseCaseIEnumerableAsync<PedidoRequest, PedidoDetalhadoPorSenhaResponse>>();
            _getAlluseCase = new Mock<IUseCaseIEnumerableAsync<IEnumerable<PedidoDetalhadoResponse>>>();
            _alteraStatusUseCase = new Mock<IUseCaseAsync<PedidoAlteraStatusRequest>>();
            _logger = new Mock<ILogger<PedidoController>>();
            _controller = new PedidoController(_getBySenhaCase.Object, _getAlluseCase.Object, _alteraStatusUseCase.Object);
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
        public async Task GetAllAsync_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var exceptionMessage = "Test exception";
            _getAlluseCase.Setup(x => x.ExecuteAsync()).Throws(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetAllAsync();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(exceptionMessage, badRequestResult.Value);
        }

        [Fact]
        public async Task GetPedidoBySenha_ShouldReturnOk_WhenPedidoExists()
        {
            // Arrange
            var pedido = new PedidoDetalhadoPorSenhaResponse();
            var request = new PedidoRequest { Senha = "1001" };
            _getBySenhaCase.Setup(x => x.ExecuteAsync(It.IsAny<PedidoRequest>())).ReturnsAsync(pedido);

            // Act
            var result = await _controller.GetPedidoBySenha(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PedidoDetalhadoPorSenhaResponse>(okResult.Value);
            Assert.Equal(pedido, returnValue);
        }

        [Fact]
        public async Task GetPedidoBySenha_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var exceptionMessage = "Test exception";
            var request = new PedidoRequest { Senha = "1001" };
            _getBySenhaCase.Setup(x => x.ExecuteAsync(It.IsAny<PedidoRequest>())).Throws(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetPedidoBySenha(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(exceptionMessage, badRequestResult.Value);
        }

        [Fact]
        public async Task Put_ShouldReturnOk_WhenNoExceptionIsThrown()
        {
            // Arrange
            var request = new PedidoAlteraStatusRequest();
            _alteraStatusUseCase.Setup(x => x.ExecuteAsync(It.IsAny<PedidoAlteraStatusRequest>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Put("1", request);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Put_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var exceptionMessage = "Test exception";
            var request = new PedidoAlteraStatusRequest();
            _alteraStatusUseCase.Setup(x => x.ExecuteAsync(It.IsAny<PedidoAlteraStatusRequest>())).Throws(new Exception(exceptionMessage));

            // Act
            var result = await _controller.Put("1", request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(exceptionMessage, badRequestResult.Value);
        }
    }
}