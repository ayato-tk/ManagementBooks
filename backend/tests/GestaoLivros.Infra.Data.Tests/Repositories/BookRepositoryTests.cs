using GestaoLivros.Domain.Entities;
using GestaoLivros.Infra.Data.Context;
using GestaoLivros.Infra.Data.Repositories;

namespace GestaoLivros.Infra.Data.Tests.Repositories;

public class BookRepositoryTests
{
    private static BookRepository CreateRepository(ApplicationDbContext context) => new(context);
    
    [Fact]
    public async Task ExistsByGenreIdAsync_ShouldReturnBoolean_WhenGenreExists()
    {
        // Arrange
        var options = TestDbContext.CreateNewContextOptions();
        await using var context = new ApplicationDbContext(options);
        var repo = CreateRepository(context);
        var book = new Book("Livro1", "1234567890", "Autor", "Sinopse", 1, 1, 1);
        await repo.AddAsync(book);
        await repo.SaveChangesAsync();
        
        // Act
        var exists = await repo.ExistsByGenreIdAsync(1);
        var notExists = await repo.ExistsByGenreIdAsync(999);
        
        // Assert
        Assert.True(exists);
        Assert.False(notExists);
    }
    
    [Fact]
    public async Task ExistsByPublisherIdAsync_ShouldReturnBoolean_WhenPublisherExists()
    {
        // Arrange
        var options = TestDbContext.CreateNewContextOptions();
        await using var context = new ApplicationDbContext(options);
        var repo = CreateRepository(context);
        var book = new Book("Livro1", "1234567890", "Autor", "Sinopse", 1, 1, 1);
        await repo.AddAsync(book);
        await repo.SaveChangesAsync();
        
        // Act
        var exists = await repo.ExistsByPublisherIdAsync(1);
        var notExists = await repo.ExistsByPublisherIdAsync(999);
        
        // Assert
        Assert.True(exists);
        Assert.False(notExists);
    }
}