using FluentValidation.TestHelper;
using GestaoLivros.Application.Requests.Books.Commands;
using GestaoLivros.Application.Requests.Books.Validations;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Entities;
using GestaoLivros.Domain.Interfaces;
using Moq;

namespace GestaoLivros.Application.Tests.Requests.Books.Validations;

public class UpdateBookCommandValidatorTests
{
    private readonly Mock<IBookRepository> _bookRepoMock = new();
    private readonly Mock<IGenreRepository> _genreRepoMock = new();
    private readonly Mock<IPublisherRepository> _publisherRepoMock = new();
    private readonly Mock<ICurrentUserService> _userServiceMock = new();
    private readonly UpdateBookCommandValidator _validator;

    public UpdateBookCommandValidatorTests()
    {
        _userServiceMock.Setup(u => u.UserId).Returns(1);

        _bookRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<Func<IQueryable<Book>, IQueryable<Book>>>()))
            .ReturnsAsync((Book)null!);
        _genreRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(),
                It.IsAny<Func<IQueryable<Domain.Entities.Genre>, IQueryable<Domain.Entities.Genre>>>()))
            .ReturnsAsync((Domain.Entities.Genre)null!);
        _publisherRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(),
                It.IsAny<Func<IQueryable<Domain.Entities.Publisher>, IQueryable<Domain.Entities.Publisher>>>()))
            .ReturnsAsync((Domain.Entities.Publisher)null!);

        _validator = new UpdateBookCommandValidator(
            _userServiceMock.Object, _genreRepoMock.Object, _publisherRepoMock.Object, _bookRepoMock.Object);
    }

    [Fact]
    public async Task Handle_HaveError_WhenBookDoesNotExist()
    {
        // Arrange
        var command = new UpdateBookCommand { Id = 999 };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Id)
            .WithErrorMessage("Livro é inválido");
    }

    [Fact]
    public async Task Handle_HaveError_WhenGenreDoesNotExist()
    {
        // Arrange
        _bookRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<Func<IQueryable<Book>, IQueryable<Book>>>()))
            .ReturnsAsync(new Book("Livro", "1234567890", "Autor", "Sinopse", 1, 1, 1) { Id = 1 });

        var command = new UpdateBookCommand { Id = 1, GenreId = 999 };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.GenreId)
            .WithErrorMessage("O Gênero é inválido");
    }

    [Fact]
    public async Task Handle_HaveError_WhenPublisherDoesNotExist()
    {
        // Arrange
        _bookRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<Func<IQueryable<Book>, IQueryable<Book>>>()))
            .ReturnsAsync(new Book("Livro", "1234567890", "Autor", "Sinopse", 1, 1, 1) { Id = 1 });

        var command = new UpdateBookCommand { Id = 1, PublisherId = 999 };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.PublisherId)
            .WithErrorMessage("Publicadora é inválida");
    }

    [Fact]
    public async Task Handle_NotHaveError_WhenCommandIsValid()
    {
        // Arrange
        _bookRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<Func<IQueryable<Book>, IQueryable<Book>>>()))
            .ReturnsAsync(new Book("Livro", "1234567890", "Autor", "Sinopse", 1, 1, 1) { Id = 1 });
        _genreRepoMock.Setup(r =>
                r.GetByIdAsync(1,
                    It.IsAny<Func<IQueryable<Domain.Entities.Genre>, IQueryable<Domain.Entities.Genre>>>()))
            .ReturnsAsync(new Domain.Entities.Genre() { Id = 1, Name = "Teste" });
        _publisherRepoMock.Setup(r => r.GetByIdAsync(1,
                It.IsAny<Func<IQueryable<Domain.Entities.Publisher>, IQueryable<Domain.Entities.Publisher>>>()))
            .ReturnsAsync(new Domain.Entities.Publisher() { Id = 1, Name = "Teste" });

        var command = new UpdateBookCommand
        {
            Id = 1,
            Title = "Livro Atualizado",
            Author = "Autor Atualizado",
            Isbn = "1234567890123",
            GenreId = 1,
            PublisherId = 1,
            Synopsis = "Sinopse Atualizada"
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}