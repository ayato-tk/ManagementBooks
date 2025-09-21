using AutoMapper;
using GestaoLivros.Application.Requests.Books.Commands;
using GestaoLivros.Application.Requests.Books.DTOs;
using GestaoLivros.Application.Requests.Books.Handlers;
using GestaoLivros.Application.Services.Interfaces;
using GestaoLivros.Domain.Entities;
using GestaoLivros.Domain.Interfaces;
using Moq;

namespace GestaoLivros.Application.Tests.Requests.Books.Handlers;

public class CreateBookCommandHandlerTests
{
    [Fact]
        public async Task Handle_ShouldCreateBook_ReturnBookResponseDto()
        {
            // Arrange
            var repoMock = new Mock<IBookRepository>();
            var userServiceMock = new Mock<ICurrentUserService>();
            var mapperMock = new Mock<IMapper>();

            userServiceMock.Setup(u => u.UserId).Returns(1);
            
            mapperMock.Setup(m => m.Map<BookResponseDto>(It.IsAny<Book>()))
                      .Returns((Book b) => new BookResponseDto
                      {
                          Title = b.Title!,
                          Author = b.Author,
                          Isbn = b.ISBN!,
                          GenreId = b.GenreId,
                          PublisherId = b.PublisherId
                      });

            var handler = new CreatBookHandler(repoMock.Object, userServiceMock.Object, mapperMock.Object);

            var command = new CreateBookCommand
            {
                Title = "Livro Teste",
                Isbn = "1234567890",
                Author = "Autor Teste",
                Synopsis = "Sinopse teste",
                GenreId = 1,
                PublisherId = 1
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Livro Teste", result.Title);
            Assert.Equal("Autor Teste", result.Author);
            Assert.Equal("1234567890", result.Isbn);
            Assert.Equal(1, result.GenreId);
            Assert.Equal(1, result.PublisherId);
        }
}