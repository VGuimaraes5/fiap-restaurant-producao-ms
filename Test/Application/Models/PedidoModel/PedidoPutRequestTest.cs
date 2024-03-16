using Xunit;
using Application.Models.PedidoModel;

namespace Application.Models.PedidoModel
{
    public class PedidoPutRequestTests
    {
        [Fact]
        public void TestPedidoPutRequestProperties()
        {
            var model = new PedidoPutRequest
            {
                Id = "123",
                Status = 1
            };

            Assert.Equal("123", model.Id);
            Assert.Equal(1, model.Status);
        }
    }
}