using AutoMapper;
using GestaoLivros.Application.Requests.Books.Commands;
using GestaoLivros.Application.Requests.Books.DTOs;
using GestaoLivros.Application.Requests.Books.Handlers;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Entities;
using GestaoLivros.Domain.Interfaces;
using Moq;

namespace GestaoLivros.Application.Tests.Requests.Books.Handlers;

public class UpdateBookCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldUpdateBook_ReturnUpdatedBookResponseDto()
    {
        // Arrange
        var repoMock = new Mock<IBookRepository>();
        var userServiceMock = new Mock<ICurrentUserService>();
        var mapperMock = new Mock<IMapper>();

        var book = new Book("Livro", "1234567890", "Autor", "Sinopse", 1, 1, 1) { Id = 1 };

        repoMock.Setup(r => r.GetByIdAsync(book.Id, It.IsAny<Func<IQueryable<Book>, IQueryable<Book>>>()))
            .ReturnsAsync(book);

        mapperMock.Setup(m => m.Map<BookResponseDto>(It.IsAny<Book>()))
            .Returns((Book b) => new BookResponseDto { Id = b.Id, Title = b.Title!, Author = b.Author });

        var handler = new UpdateBookCommandHandler(repoMock.Object, userServiceMock.Object, mapperMock.Object);

        var command = new UpdateBookCommand
        {
            Id = book.Id,
            Title = "Livro Atualizado",
            Author = "Autor Atualizado",
            Isbn = "1234567890",
            Synopsis = "Nova Sinopse",
            GenreId = 1,
            PublisherId = 1
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("Livro Atualizado", result.Title);
    }
}