using GestaoLivros.Application.Requests.User.Commands;
using GestaoLivros.Application.Requests.User.Handlers;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using Moq;

namespace GestaoLivros.Application.Tests.Requests.User.Handlers;

public class SignInUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnToken_WhenUserExists()
    {
        // Arrange
        const string email = "teste@email.com";
        const string expectedToken = "jwt-token";
        var user = new Domain.Entities.User { Id = 1, Email = email, Name = "Teste", PasswordHash = "hashed" };

        var userRepoMock = new Mock<IUserRepository>();
        var tokenServiceMock = new Mock<ITokenService>();

        userRepoMock
            .Setup(r => r.GetByEmailAsync(email))
            .ReturnsAsync(user);

        tokenServiceMock
            .Setup(s => s.GenerateToken(user.Id.ToString()))
            .Returns(expectedToken);

        var handler = new SignInUserCommandHandler(tokenServiceMock.Object, userRepoMock.Object);
        var command = new SignInUserCommand(email, "senha");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(expectedToken, result);
        userRepoMock.Verify(r => r.GetByEmailAsync(email), Times.Once);
        tokenServiceMock.Verify(s => s.GenerateToken(user.Id.ToString()), Times.Once);
    }
}