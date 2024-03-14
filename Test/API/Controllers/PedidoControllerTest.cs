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
    }
}