using FluentValidation;
using FluentValidation.Results;
using GestaoLivros.Application;
using GestaoLivros.Application.Requests.Books.Commands;
using GestaoLivros.Application.Requests.Books.DTOs;
using GestaoLivros.Application.Requests.Books.Queries;
using GestaoLivros.Presentation.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GestaoLivros.Presentation.Tests.Controllers;

public class BookControllerTests
{
    private readonly Mock<IMediator> _mediatorMock = new();
    private readonly Mock<IValidator<CreateBookCommand>> _createValidatorMock = new();
    private readonly Mock<IValidator<UpdateBookCommand>> _updateValidatorMock = new();
    private readonly Mock<IValidator<DeleteBookCommand>> _deleteValidatorMock = new();

    private readonly BookResponseDto _book = new() { Title = "Livro", Isbn = "1234567890", Author = "Teste" };

    [Fact]
    public async Task GetBooks_ShouldReturnOk()
    {
        // Arrange
        var query = new GetPaginatedBooksQuery { Page = 1, PageSize = 10 };
        var books = new PaginatedResponse<BookResponseDto>();

        _mediatorMock.Setup(m => m.Send(query, CancellationToken.None)).ReturnsAsync(books);

        var controller = new BookController(_mediatorMock.Object);

        // Act
        var result = await controller.GetBooks(query);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(books, okResult.Value);
    }

    [Fact]
    public async Task GetBooksReport_ShouldReturnFile()
    {
        // Arrange
        var pdfBytes = new byte[] { 1, 2, 3 };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetBooksReportQuery>(), CancellationToken.None))
            .ReturnsAsync(pdfBytes);

        var controller = new BookController(_mediatorMock.Object);

        // Act
        var result = await controller.GetBooksReport();

        // Assert
        var fileResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal("application/pdf", fileResult.ContentType);
        Assert.Equal("relatorio_livros.pdf", fileResult.FileDownloadName);
        Assert.Equal(pdfBytes, fileResult.FileContents);
    }

    [Fact]
    public async Task GetBook_ShouldReturnOk()
    {
        // Arrange
        var query = new GetBookQuery(1);

        _mediatorMock.Setup(m => m.Send(query, CancellationToken.None)).ReturnsAsync(_book);

        var controller = new BookController(_mediatorMock.Object);

        // Act
        var result = await controller.GetBook(query);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(_book, okResult.Value);
    }

    [Fact]
    public async Task CreateBook_ShouldReturnOk_WhenValid()
    {
        // Arrange
        var command = new CreateBookCommand
        {
            Title = _book.Title,
            Author = _book.Author,
            Isbn = _book.Isbn,
            CoverImagePath = _book.CoverImagePath,
            PublisherId = _book.PublisherId
        };
        _createValidatorMock.Setup(v => v.ValidateAsync(command, CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        _mediatorMock.Setup(m => m.Send(command, CancellationToken.None)).ReturnsAsync(_book);

        var controller = new BookController(_mediatorMock.Object);

        // Act
        var result = await controller.CreateBook(command, _createValidatorMock.Object);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(_book, okResult.Value);
    }

    [Fact]
    public async Task UpdateBook_ShouldReturnOk_WhenValid()
    {
        // Arrange
        var command = new UpdateBookCommand { Id = 1, Title = _book.Title };
        _updateValidatorMock.Setup(v => v.ValidateAsync(command, CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        _mediatorMock.Setup(m => m.Send(command, CancellationToken.None)).ReturnsAsync(_book);

        var controller = new BookController(_mediatorMock.Object);

        // Act
        var result = await controller.UpdateBook(command, _updateValidatorMock.Object);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(_book, okResult.Value);
    }

    [Fact]
    public async Task DeleteBook_ShouldReturnOk_WhenValid()
    {
        // Arrange
        var command = new DeleteBookCommand { Id = 1 };
        _deleteValidatorMock.Setup(v => v.ValidateAsync(command, CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        _mediatorMock.Setup(m => m.Send(command, CancellationToken.None)).ReturnsAsync(_book);

        var controller = new BookController(_mediatorMock.Object);

        // Act
        var result = await controller.DeleteBook(command, _deleteValidatorMock.Object);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(_book, okResult.Value);
    }
}