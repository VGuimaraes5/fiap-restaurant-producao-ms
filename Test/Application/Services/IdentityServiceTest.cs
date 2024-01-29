using Application.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using Xunit;

namespace Application.Services
{
    public class IdentityServiceTests
    {
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly IdentityService _identityService;

        public IdentityServiceTests()
        {
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _identityService = new IdentityService(_mockHttpContextAccessor.Object);
        }

        [Fact]
        public void GetUserId_ShouldReturnUserId()
        {
            // Arrange
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, "123")}));
            _mockHttpContextAccessor.Setup(x => x.HttpContext.User).Returns(claimsPrincipal);

            // Act
            var userId = _identityService.GetUserId();

            // Assert
            Assert.Equal("123", userId);
        }
    }
}