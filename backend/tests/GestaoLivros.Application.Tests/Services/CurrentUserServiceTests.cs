using System.Security.Claims;
using GestaoLivros.Application.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Moq;

namespace GestaoLivros.Application.Tests.Services;

public class CurrentUserServiceTests
{
    [Fact]
    public void UserId_ShouldReturnClaimValue_WhenUserIsAuthenticated()
    {
        // Arrange
        const int userId = 123;
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, userId.ToString()) };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(c => c.User).Returns(principal);

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);

        var service = new CurrentUserService(httpContextAccessorMock.Object);

        // Act
        var result = service.UserId;

        // Assert
        Assert.Equal(userId, result);
    }

    [Fact]
    public void UserId_ShouldThrowUnauthorizedAccessException_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(c => c.User).Returns(new ClaimsPrincipal(new ClaimsIdentity()));

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);

        var service = new CurrentUserService(httpContextAccessorMock.Object);

        // Act & Assert
        Assert.Throws<UnauthorizedAccessException>(() => { var _ = service.UserId; });
    }
}