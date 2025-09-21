using FluentValidation.TestHelper;
using GestaoLivros.Application.Requests.Books.Commands;
using GestaoLivros.Application.Requests.Books.Validations;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Entities;
using GestaoLivros.Domain.Interfaces;
using Moq;

namespace GestaoLivros.Application.Tests.Requests.Books.Validations;

public class DeleteBookCommandValidatorTests
{
    private readonly Mock<IBookRepository> _bookRepoMock = new();
    private readonly Mock<ICurrentUserService> _userServiceMock = new();
    private readonly DeleteBookCommandValidator _validator;

    public DeleteBookCommandValidatorTests()
    {
        _userServiceMock.Setup(u => u.UserId).Returns(1);

        _bookRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<Func<IQueryable<Book>, IQueryable<Book>>>()))
            .ReturnsAsync((Book)null!);

        _validator = new DeleteBookCommandValidator(_bookRepoMock.Object, _userServiceMock.Object);
    }

    [Fact]
    public async Task Handle_HaveError_WhenBookDoesNotExist()
    {
        // Arrange
        var command = new DeleteBookCommand { Id = 999 };

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Id)
            .WithErrorMessage("O Livro é inválido");
    }

    [Fact]
    public async Task Handle_NotHaveError_WhenBookExists()
    {
        // Arrange
        _bookRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<Func<IQueryable<Book>, IQueryable<Book>>>()))
            .ReturnsAsync(new Book("Livro", "1234567890", "Autor", "Sinopse", 1, 1, 1) { Id = 1 });

        var command = new DeleteBookCommand { Id = 1 };
        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Id);
    }
}