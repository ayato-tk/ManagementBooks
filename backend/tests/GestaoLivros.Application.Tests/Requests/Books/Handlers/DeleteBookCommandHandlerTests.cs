using AutoMapper;
using GestaoLivros.Application.Requests.Books.Commands;
using GestaoLivros.Application.Requests.Books.DTOs;
using GestaoLivros.Application.Requests.Books.Handlers;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Entities;
using GestaoLivros.Domain.Interfaces;
using Moq;

namespace GestaoLivros.Application.Tests.Requests.Books.Handlers;

public class DeleteBookCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldDeleteBook_ReturnBookResponseDto()
    {
        // Arrange
        var repoMock = new Mock<IBookRepository>();
        var userServiceMock = new Mock<ICurrentUserService>();
        userServiceMock.Setup(u => u.UserId).Returns(1);

        var mapperMock = new Mock<IMapper>();
        mapperMock
            .Setup(m => m.Map<BookResponseDto>(It.IsAny<Book>()))
            .Returns((Book b) => new BookResponseDto { Title = b.Title! });

        var book = new Book("Livro", "1234567890", "Autor", "Sinopse", 1, 1, 1) { Id = 1 };

        repoMock.Setup(r => r.GetByIdAsync(book.Id, It.IsAny<Func<IQueryable<Book>, IQueryable<Book>>>()))
            .ReturnsAsync(book);

        var command = new DeleteBookCommand()
        {
            Id = 1
        };

        var handler = new DeleteBookCommandHandler(
            repoMock.Object,
            userServiceMock.Object,
            mapperMock.Object
        );

        // Act
        var result = await handler.Handle(command, default);
        
        // Assert
        Assert.NotEqual(command.Title, result.Title);
    }
}