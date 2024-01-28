using Application.Models.PedidoModel;
using Application.UseCases.PedidoUseCase;
using AutoMapper;
using Domain.Entities;
using Domain.Gateways;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace Test.Application.UseCases.PedidoUseCase
{
    public class GetAllPedidoUseCaseAsyncTests
    {
        private readonly Mock<IPedidoGateway> _gateway;
        private readonly Mock<IMapper> _mapper;
        private readonly MemoryCache _memoryCache;

        public GetAllPedidoUseCaseAsyncTests()
        {
            _gateway = new Mock<IPedidoGateway>();
            _mapper = new Mock<IMapper>();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnAllPedidos()
        {
            // Arrange
            var pedidos = new List<Pedido> { new Pedido() };
            _gateway.Setup(x => x.GetPedidosDetalhadosAsync()).ReturnsAsync(pedidos);

            var mappedResult = new List<PedidoDetalhadoResponse>();
            _mapper.Setup(m => m.Map<IEnumerable<PedidoDetalhadoResponse>>(It.IsAny<IEnumerable<Pedido>>())).Returns(mappedResult);

            var useCase = new GetAllPedidoUseCaseAsync(_gateway.Object, _mapper.Object, _memoryCache);

            // Act
            var result = await useCase.ExecuteAsync();

            // Assert
            Assert.NotNull(result);
        }
    }
}