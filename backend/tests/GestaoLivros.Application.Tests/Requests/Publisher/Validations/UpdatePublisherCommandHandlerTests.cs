using AutoMapper;
using GestaoLivros.Application.Requests.Genre.Commands;
using GestaoLivros.Application.Requests.Genre.DTOs;
using GestaoLivros.Application.Requests.Genre.Handlers;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using Moq;

namespace GestaoLivros.Application.Tests.Requests.Publisher.Validations;

public class UpdatePublisherCommandHandlerTests
{
    private readonly Mock<IGenreRepository> _genreRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateGenreCommandHandler _handler;

    public UpdatePublisherCommandHandlerTests()
    {
        _genreRepositoryMock = new Mock<IGenreRepository>();
        var currentUserServiceMock = new Mock<ICurrentUserService>();
        _mapperMock = new Mock<IMapper>();

        currentUserServiceMock.Setup(x => x.UserId).Returns(1);

        _handler = new UpdateGenreCommandHandler(
            _genreRepositoryMock.Object,
            currentUserServiceMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_HaveError_WhenGenreNotFound()
    {
        // Arrange
        var command = new UpdateGenreCommand { Id = 10, Name = "Novo Nome" };

        _genreRepositoryMock
            .Setup(r => r.GetByIdAsync(command.Id,
                It.IsAny<Func<IQueryable<Domain.Entities.Genre>, IQueryable<Domain.Entities.Genre>>>()))
            .ReturnsAsync((Domain.Entities.Genre)null!);

        // Act
        var handle = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(handle);
        Assert.Equal("Gênero não encontrado", exception.Message);
    }

    [Fact]
    public async Task Handle_NotHaveError_WhenCommandIsValid()
    {
        // Arrange
        var command = new UpdateGenreCommand { Id = 20, Name = "Romance Atualizado" };
        var existingGenre = new Domain.Entities.Genre { Id = 20, UserId = 1, Name = "Romance Antigo" };
        var mappedDto = new GenreResponseDto { Id = 20, Name = "Romance Atualizado" };

        _genreRepositoryMock
            .Setup(r => r.GetByIdAsync(command.Id,
                It.IsAny<Func<IQueryable<Domain.Entities.Genre>, IQueryable<Domain.Entities.Genre>>>()))
            .ReturnsAsync(existingGenre);

        _mapperMock
            .Setup(m => m.Map<GenreResponseDto>(It.IsAny<Domain.Entities.Genre>()))
            .Returns(mappedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(command.Name, existingGenre.Name);
        Assert.Equal(mappedDto.Name, result.Name);
    }
}