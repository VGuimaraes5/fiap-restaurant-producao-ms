using Application.Models.PedidoModel;
using Application.UseCases.PedidoUseCase;
using AutoMapper;
using Domain.Entities;
using Domain.Gateways;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace Test.Application.UseCases.PedidoUseCase
{
    public class GetPedidoBySenhaUseCaseAsyncTests
    {
        private readonly Mock<IPedidoGateway> _gateway;
        private readonly Mock<IMapper> _mapper;
        private readonly MemoryCache _memoryCache;

        public GetPedidoBySenhaUseCaseAsyncTests()
        {
            _gateway = new Mock<IPedidoGateway>();
            _mapper = new Mock<IMapper>();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnMappedResult_WhenPedidoExists()
        {
            // Arrange
            var request = new PedidoRequest { Senha = 1001 };
            var pedido = new Pedido();
            _gateway.Setup(x => x.GetPedidoBySenhaUseCaseAsync(request.Senha)).ReturnsAsync(pedido);

            var mappedResult = new PedidoDetalhadoPorSenhaResponse();
            _mapper.Setup(m => m.Map<PedidoDetalhadoPorSenhaResponse>(It.IsAny<Pedido>())).Returns(mappedResult);

            var useCase = new GetPedidoBySenhaUseCaseAsync(_gateway.Object, _mapper.Object, _memoryCache);

            // Act
            var result = await useCase.ExecuteAsync(request);

            // Assert
            Assert.Equal(mappedResult, result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnMappedResult_WhenPedidoIsNotInCache()
        {
            // Arrange
            var request = new PedidoRequest { Senha = 1001 };
            var pedido = new Pedido();
            _gateway.Setup(x => x.GetPedidoBySenhaUseCaseAsync(request.Senha)).ReturnsAsync(pedido);

            var mappedResult = new PedidoDetalhadoPorSenhaResponse();
            _mapper.Setup(m => m.Map<PedidoDetalhadoPorSenhaResponse>(It.IsAny<Pedido>())).Returns(mappedResult);

            var useCase = new GetPedidoBySenhaUseCaseAsync(_gateway.Object, _mapper.Object, _memoryCache);

            // Act
            var result = await useCase.ExecuteAsync(request);

            // Assert
            Assert.Equal(mappedResult, result);
            _gateway.Verify(x => x.GetPedidoBySenhaUseCaseAsync(request.Senha), Times.Once);
        }
    }
}