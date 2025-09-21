using AutoMapper;
using GestaoLivros.Application.Requests.Genre.Commands;
using GestaoLivros.Application.Requests.Genre.DTOs;
using GestaoLivros.Application.Requests.Genre.Handlers;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using Moq;

namespace GestaoLivros.Application.Tests.Requests.Genre.Handlers;

public class CreateGenreCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateGenre_ThenReturnGenreResponseDto()
    {
        // Arrange
        var repoMock = new Mock<IGenreRepository>();
        var userServiceMock = new Mock<ICurrentUserService>();
        var mapperMock = new Mock<IMapper>();
        const string genreName = "Test";

        userServiceMock.Setup(u => u.UserId).Returns(1);
            
        mapperMock.Setup(m => m.Map<GenreResponseDto>(It.IsAny<Domain.Entities.Genre>()))
            .Returns((Domain.Entities.Genre b) => new GenreResponseDto
            {
                Name = genreName
            });

        var handler = new CreateGenreCommandHandler(repoMock.Object, userServiceMock.Object, mapperMock.Object);

        var command = new CreateGenreCommand(genreName);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(genreName, result.Name);
    }
}