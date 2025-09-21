using System.Linq.Expressions;
using AutoMapper;
using GestaoLivros.Application.Requests.Books.DTOs;
using GestaoLivros.Application.Requests.Books.Handlers;
using GestaoLivros.Application.Requests.Books.Queries;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Entities;
using GestaoLivros.Domain.Interfaces;
using Moq;

namespace GestaoLivros.Application.Tests.Requests.Books.Handlers;

public class GetPaginatedBooksQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldGetPaginatedBooks_ReturnPaginatedBookResponseDto()
    {
        // Arrange
        var repoMock = new Mock<IBookRepository>();
        var mapperMock = new Mock<IMapper>();
        var userServiceMock = new Mock<ICurrentUserService>();
        userServiceMock.Setup(u => u.UserId).Returns(1);

        var books = new List<Book>
        {
            new("Livro1", "1234567890", "Autor1", "Sinopse1", 1, 1, 1) { Id = 1 },
            new("Livro2", "1234567890", "Autor2", "Sinopse2", 1, 1, 1) { Id = 2 }
        };

        repoMock.Setup(r => r.GetPaginatedAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<Func<IQueryable<Book>, IQueryable<Book>>>(),
                It.IsAny<Expression<Func<Book, string>>[]>()
            ))
            .ReturnsAsync((books, books.Count));

        mapperMock.Setup(m => m.Map<List<BookResponseDto>>(books))
            .Returns(books.Select(b => new BookResponseDto { Title = b.Title! }).ToList());

        var handler = new GetPaginatedBooksQueryHandler(repoMock.Object, mapperMock.Object, userServiceMock.Object);
        var query = new GetPaginatedBooksQuery { Page = 1, PageSize = 10 };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.TotalItems);
        Assert.Equal("Livro1", result.Data[0].Title);
        Assert.Equal("Livro2", result.Data[1].Title);
    }
}