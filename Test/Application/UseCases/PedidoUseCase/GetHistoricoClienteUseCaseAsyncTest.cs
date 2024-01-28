using Application.Models.PedidoModel;
using Application.UseCases.PedidoUseCase;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Gateways;
using Domain.Services;
using Moq;

namespace Test.Application.UseCases.PedidoUseCase
{
    public class GetHistoricoClienteUseCaseAsyncTests
    {
        private readonly Mock<IPedidoGateway> _gateway;
        private readonly Mock<IIdentityService> _identityService;
        private readonly Mock<IMapper> _mapper;

        public GetHistoricoClienteUseCaseAsyncTests()
        {
            _gateway = new Mock<IPedidoGateway>();
            _identityService = new Mock<IIdentityService>();
            _mapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnMappedResult()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            _identityService.Setup(x => x.GetUserId()).Returns(userId);

            var historico = new List<Pedido> { new Pedido() };
            _gateway.Setup(x => x.GetHistoricoAsync(userId)).ReturnsAsync(historico);

            var mappedResult = new List<HistoricoClienteResponse>();
            _mapper.Setup(m => m.Map<IEnumerable<HistoricoClienteResponse>>(It.IsAny<IEnumerable<Pedido>>())).Returns(mappedResult);

            var useCase = new GetHistoricoClienteUseCaseAsync(_gateway.Object, _identityService.Object, _mapper.Object);

            // Act
            var result = await useCase.ExecuteAsync();

            // Assert
            Assert.Equal(mappedResult, result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnEmptyList_WhenNoHistoricoExists()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            _identityService.Setup(x => x.GetUserId()).Returns(userId);

            var historico = new List<Pedido>();
            _gateway.Setup(x => x.GetHistoricoAsync(userId)).ReturnsAsync(historico);

            var useCase = new GetHistoricoClienteUseCaseAsync(_gateway.Object, _identityService.Object, _mapper.Object);

            // Act
            var result = await useCase.ExecuteAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldCallGetHistoricoAsync_WithCorrectUserId()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            _identityService.Setup(x => x.GetUserId()).Returns(userId);

            var historico = new List<Pedido> { new Pedido() };
            _gateway.Setup(x => x.GetHistoricoAsync(userId)).ReturnsAsync(historico);

            var useCase = new GetHistoricoClienteUseCaseAsync(_gateway.Object, _identityService.Object, _mapper.Object);

            // Act
            await useCase.ExecuteAsync();

            // Assert
            _gateway.Verify(x => x.GetHistoricoAsync(userId), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldCallMap_WithCorrectHistorico()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            _identityService.Setup(x => x.GetUserId()).Returns(userId);

            var historico = new List<Pedido> { new Pedido() };
            _gateway.Setup(x => x.GetHistoricoAsync(userId)).ReturnsAsync(historico);

            var useCase = new GetHistoricoClienteUseCaseAsync(_gateway.Object, _identityService.Object, _mapper.Object);

            // Act
            await useCase.ExecuteAsync();

            // Assert
            _mapper.Verify(m => m.Map<IEnumerable<HistoricoClienteResponse>>(historico), Times.Once);
        }

    }

}