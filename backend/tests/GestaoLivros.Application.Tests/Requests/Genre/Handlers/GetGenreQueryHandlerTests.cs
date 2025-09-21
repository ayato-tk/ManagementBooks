using AutoMapper;
using GestaoLivros.Application.Requests.Genre.DTOs;
using GestaoLivros.Application.Requests.Genre.Handlers;
using GestaoLivros.Application.Requests.Genre.Queries;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using Moq;

namespace GestaoLivros.Application.Tests.Requests.Genre.Handlers;

public class GetGenreQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldGetGenreById_ReturnGenreResponseDto()
    {
        // Arrange
        var repoMock = new Mock<IGenreRepository>();
        var userServiceMock = new Mock<ICurrentUserService>();
        userServiceMock.Setup(u => u.UserId).Returns(1);

        var mapperMock = new Mock<IMapper>();
        mapperMock
            .Setup(m => m.Map<GenreResponseDto>(It.IsAny<Domain.Entities.Genre>()))
            .Returns((Domain.Entities.Genre b) => new GenreResponseDto() { Id = b.Id, Name = b.Name! });

        var genre = new Domain.Entities.Genre { Id = 1, Name = "Teste" };

        repoMock.Setup(r => r.GetByIdAsync(genre.Id,
                It.IsAny<Func<IQueryable<Domain.Entities.Genre>, IQueryable<Domain.Entities.Genre>>>()))
            .ReturnsAsync(genre);

        var command = new GetGenreQuery(1);

        var handler = new GetGenreQueryHandler(
            userServiceMock.Object,
            repoMock.Object,
            mapperMock.Object
        );

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(1, result.Id);
    }
}