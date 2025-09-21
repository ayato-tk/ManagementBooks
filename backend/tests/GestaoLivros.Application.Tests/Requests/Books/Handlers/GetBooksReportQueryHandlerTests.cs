using GestaoLivros.Application.Requests.Books.Handlers;
using GestaoLivros.Application.Requests.Books.Queries;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Entities;
using GestaoLivros.Domain.Interfaces;
using Moq;

namespace GestaoLivros.Application.Tests.Requests.Books.Handlers;

public class GetBooksReportQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldGetBooksReport_ReturnPdfBytes()
    {
        // Arrange
        var repoMock = new Mock<IBookRepository>();
        var userServiceMock = new Mock<ICurrentUserService>();
        var reportServiceMock = new Mock<IReportService>();

        userServiceMock.Setup(u => u.UserId).Returns(1);

        var books = new List<Book>
        {
            new Book("Livro1", "1234567890", "Autor1", "Sinopse", 1, 1, 1) { Id = 1 },
            new Book("Livro2", "1234567890", "Autor2", "Sinopse", 1, 1, 1) { Id = 2 }
        };

        repoMock.Setup(r => r.GetAllAsync(It.IsAny<Func<IQueryable<Book>, IQueryable<Book>>>()))
            .ReturnsAsync(books);

        var fakePdf = new byte[] { 1, 2, 3 };
        reportServiceMock.Setup(r => r.GenerateBooksReport(books)).Returns(fakePdf);

        var handler = new GetBooksReportQueryHandler(
            repoMock.Object,
            userServiceMock.Object,
            reportServiceMock.Object
        );

        var query = new GetBooksReportQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(fakePdf, result);
        repoMock.Verify(r => r.GetAllAsync(It.IsAny<Func<IQueryable<Book>, IQueryable<Book>>>()), Times.Once);
        reportServiceMock.Verify(r => r.GenerateBooksReport(books), Times.Once);
    }
}