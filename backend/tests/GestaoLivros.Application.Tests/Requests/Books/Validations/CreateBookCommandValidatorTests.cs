using FluentValidation.TestHelper;
using GestaoLivros.Application.Requests.Books.Commands;
using GestaoLivros.Application.Requests.Books.Validations;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Interfaces;
using Moq;

namespace GestaoLivros.Application.Tests.Requests.Books.Validations;

public class CreateBookCommandValidatorTests
{
    private readonly Mock<IGenreRepository> _genreRepoMock = new();
    private readonly Mock<IPublisherRepository> _publisherRepoMock = new();
    private readonly Mock<ICurrentUserService> _userServiceMock = new();
    private readonly CreateBookCommandValidator _validator;

    public CreateBookCommandValidatorTests()
    {
        _userServiceMock.Setup(u => u.UserId).Returns(1);

        _genreRepoMock.Setup(r =>
                r.GetByIdAsync(It.IsAny<int>(),
                    It.IsAny<Func<IQueryable<Domain.Entities.Genre>, IQueryable<Domain.Entities.Genre>>>()))
            .ReturnsAsync((Domain.Entities.Genre)null!);
        _publisherRepoMock.Setup(r =>
                r.GetByIdAsync(It.IsAny<int>(), It.IsAny<Func<IQueryable<Domain.Entities.Publisher>, IQueryable<Domain.Entities.Publisher>>>()))
            .ReturnsAsync((Domain.Entities.Publisher)null!);

        _validator =
            new CreateBookCommandValidator(_genreRepoMock.Object, _publisherRepoMock.Object, _userServiceMock.Object);
    }

    [Fact]
    public async Task Should_HaveError_WhenTitleIsEmpty()
    {
        // Arrange
        var command = new CreateBookCommand { Title = "" };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Title);
    }

    [Fact]
    public async Task Should_HaveError_WhenISBNIsInvalid()
    {
        // Arrange
        var command = new CreateBookCommand { Isbn = "123" };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Isbn);
    }

    [Fact]
    public async Task Should_HaveError_WhenGenreDoesNotExist()
    {
        // Arrange
        _genreRepoMock.Setup(r => r.GetByIdAsync(999,
                It.IsAny<Func<IQueryable<Domain.Entities.Genre>, IQueryable<Domain.Entities.Genre>>>()))
            .ReturnsAsync((Domain.Entities.Genre)null!);

        var command = new CreateBookCommand { GenreId = 999 };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.GenreId)
            .WithErrorMessage("Gênero é inválido");
    }

    [Fact]
    public async Task Handle_HaveError_WhenPublisherDoesNotExist()
    {
        // Arrange
        _publisherRepoMock.Setup(r =>
                r.GetByIdAsync(999, It.IsAny<Func<IQueryable<Domain.Entities.Publisher>, IQueryable<Domain.Entities.Publisher>>>()))
            .ReturnsAsync((Domain.Entities.Publisher)null!);

        var command = new CreateBookCommand { PublisherId = 999 };

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
        _genreRepoMock.Setup(r =>
                r.GetByIdAsync(1,
                    It.IsAny<Func<IQueryable<Domain.Entities.Genre>, IQueryable<Domain.Entities.Genre>>>()))
            .ReturnsAsync(new Domain.Entities.Genre() { Id = 1, Name = "Teste" });
        _publisherRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<Func<IQueryable<Domain.Entities.Publisher>, IQueryable<Domain.Entities.Publisher>>>()))
            .ReturnsAsync(new Domain.Entities.Publisher() { Id = 1, Name = "Teste" });

        var command = new CreateBookCommand
        {
            Title = "Livro",
            Isbn = "1234567890",
            Author = "Autor",
            Synopsis = "Sinopse",
            GenreId = 1,
            PublisherId = 1
        };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}