using AutoMapper;
using GestaoLivros.Application.Requests.Genre.Commands;
using GestaoLivros.Application.Requests.Genre.DTOs;
using GestaoLivros.Application.Requests.Genre.Handlers;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using Moq;

namespace GestaoLivros.Application.Tests.Requests.Genre.Handlers;

public class UpdateGenreCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldUpdateGenre_ReturnUpdatedGenreResponseDto()
    {
        // Arrange
        var repoMock = new Mock<IGenreRepository>();
        var userServiceMock = new Mock<ICurrentUserService>();
        var mapperMock = new Mock<IMapper>();

        var genre = new Domain.Entities.Genre { Id = 1, Name = "Teste"};

        repoMock.Setup(r => r.GetByIdAsync(genre.Id, It.IsAny<Func<IQueryable<Domain.Entities.Genre>, IQueryable<Domain.Entities.Genre>>>()))
            .ReturnsAsync(genre);

        mapperMock.Setup(m => m.Map<GenreResponseDto>(It.IsAny<Domain.Entities.Genre>()))
            .Returns((Domain.Entities.Genre b) => new GenreResponseDto { Id = b.Id, Name = b.Name!});

        var handler = new UpdateGenreCommandHandler(repoMock.Object, userServiceMock.Object, mapperMock.Object);

        var command = new UpdateGenreCommand
        {
            Id = genre.Id,
            Name = genre.Name
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("Teste", genre.Name);
    }
}