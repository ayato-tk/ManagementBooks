using GestaoLivros.Domain.Entities;
using GestaoLivros.Domain.Exceptions;

namespace GestaoLivros.Domain.Tests.Entities;

public class BookTests
{
    [Fact]
    public void Constructor_MustCreateBook_WhenValidParameters()
    {
        // Arrange
        const string title = "Meu Livro";
        const string isbn = "1234567890";
        const string author = "Autor Teste";
        const string synopsis = "Sinopse legal";
        const int genreId = 1;
        const int publisherId = 1;
        const int userId = 42;

        // Act
        var book = new Book(title, isbn, author, synopsis, genreId, publisherId, userId);

        // Assert
        Assert.Equal(title, book.Title);
        Assert.Equal(isbn, book.ISBN);
        Assert.Equal(author, book.Author);
        Assert.Equal(synopsis, book.Synopsis);
        Assert.Equal(genreId, book.GenreId);
        Assert.Equal(publisherId, book.PublisherId);
        Assert.Equal(userId, book.UserId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Constructor_MustLaunchException_WhenTitleInvalid(string? tituloInvalido)
    {
        // Act
        var ex = Assert.Throws<DomainException>(() =>
            new Book(tituloInvalido, "1234567890", "Autor", "Sinopse", 1, 1, 42)
        );
        // Assert
        Assert.Equal("Título inválido", ex.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Constructor_MustLaunchException_WhenISBNInvalid(string? isbnInvalido)
    {
        // Act
        var ex = Assert.Throws<DomainException>(() =>
            new Book("Título", isbnInvalido, "Autor", "Sinopse", 1, 1, 42)
        );
        // Assert
        Assert.Equal("ISBN inválido", ex.Message);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("12345678901234")]
    public void Constructor_MustLaunchException_WhenISBNLengthInvalid(string isbn)
    {
        // Act
        var ex = Assert.Throws<DomainException>(() =>
            new Book("Título", isbn, "Autor", "Sinopse", 1, 1, 42)
        );
        // Assert
        Assert.Equal("ISBN deve ter 10 a 13 caracteres", ex.Message);
    }

    [Fact]
    public void Constructor_MustLaunchException_WhenGenderInvalid()
    {
        // Act
        var ex = Assert.Throws<DomainException>(() =>
            new Book("Título", "1234567890", "Autor", "Sinopse", 0, 1, 42)
        );
        // Assert
        Assert.Equal("Gênero é obrigatório", ex.Message);
    }

    [Fact]
    public void Constructor_MustLaunchException_WhenPublisherInvalid()
    {
        // Act
        var ex = Assert.Throws<DomainException>(() =>
            new Book("Título", "1234567890", "Autor", "Sinopse", 1, 0, 42)
        );
        // Assert
        Assert.Equal("Publicadora é obrigatório", ex.Message);
    }
}