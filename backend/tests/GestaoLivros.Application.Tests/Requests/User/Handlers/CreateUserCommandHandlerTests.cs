using AutoMapper;
using GestaoLivros.Application.Requests.User.Commands;
using GestaoLivros.Application.Requests.User.DTOs;
using GestaoLivros.Application.Requests.User.Handlers;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using Moq;

namespace GestaoLivros.Application.Tests.Requests.User.Handlers;

public class CreateUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateUser_ThenReturnUserResponseDto()
    {
        // Arrange
        var repoMock = new Mock<IUserRepository>();
        var userServiceMock = new Mock<ICurrentUserService>();
        var mapperMock = new Mock<IMapper>();
        const string userName = "Test";

        userServiceMock.Setup(u => u.UserId).Returns(1);

        mapperMock.Setup(m => m.Map<UserResponseDto>(It.IsAny<Domain.Entities.User>()))
            .Returns((Domain.Entities.User b) => new UserResponseDto
            {
                Id = b.Id,
                Name = b.Name,
                Email = b.Email
            });

        var handler = new CreateUserCommandHandler(repoMock.Object, mapperMock.Object);

        var command = new CreateUserCommand(userName, "teste@gmail.com", "test", DateTime.Now);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userName, result.Name);
    }
}