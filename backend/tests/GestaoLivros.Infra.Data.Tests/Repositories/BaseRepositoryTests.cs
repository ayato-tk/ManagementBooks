using GestaoLivros.Domain.Entities;
using GestaoLivros.Infra.Data.Context;
using GestaoLivros.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GestaoLivros.Infra.Data.Tests.Repositories;

public class BaseRepositoryTests
{
    private static BaseRepository<Book> CreateRepository(DbContext context) => new(context);

    [Fact]
    public async Task AddAsync_MustAddBook()
    {
        // Arrange
        var options = TestDbContext.CreateNewContextOptions();
        await using var context = new ApplicationDbContext(options);
        var repo = CreateRepository(context);
        var book = new Book("Livro1", "1234567890", "Autor", "Sinopse", 1, 1, 1);
        
        // Act
        await repo.AddAsync(book);
        await repo.SaveChangesAsync();

        // Assert
        var saved = await context.Set<Book>().FirstOrDefaultAsync();
        Assert.NotNull(saved);
        Assert.Equal("Livro1", saved.Title);
    }

    [Fact]
    public async Task GetAllAsync_MustReturnAllBooks()
    {
        // Arrange
        var options = TestDbContext.CreateNewContextOptions();
        await using var context = new ApplicationDbContext(options);
        var repo = CreateRepository(context);
        await repo.AddAsync(new Book("Livro1", "1234567890", "Autor", "Sinopse", 1, 1, 1));
        await repo.AddAsync(new Book("Livro2", "1234567890123", "Autor2", "Sinopse", 1, 1, 1));
        await repo.SaveChangesAsync();

        // Act
        var all = await repo.GetAllAsync();

        // Assert
        Assert.Equal(2, all.Count);
    }

    [Fact]
    public async Task GetByIdAsync_MustReturnBookById()
    {
        // Arrange
        var options = TestDbContext.CreateNewContextOptions();
        await using var context = new ApplicationDbContext(options);
        var repo = CreateRepository(context);
        var book = new Book("Livro1", "1234567890", "Autor", "Sinopse", 1, 1, 1);
        await repo.AddAsync(book);
        await repo.SaveChangesAsync();

        // Act
        var found = await repo.GetByIdAsync(book.Id, null);

        // Assert
        Assert.NotNull(found);
        Assert.Equal("Livro1", found.Title);
    }

    [Fact]
    public async Task GetPaginatedAsync_MustReturnItemsAndPagination()
    {
        // Arrange
        var options = TestDbContext.CreateNewContextOptions();
        await using var context = new ApplicationDbContext(options);
        var repo = CreateRepository(context);
        for (var i = 1; i <= 10; i++)
        {
            await repo.AddAsync(new Book($"Livro{i}", $"123456789{i % 10}0", "Autor", "Sinopse", 1, 1, 1));
        }

        await repo.SaveChangesAsync();

        // Act
        var (items, totalCount) = await repo.GetPaginatedAsync(2, 3);

        // Assert
        Assert.Equal(10, totalCount);
        Assert.Equal(3, items.Count);
        Assert.Equal("Livro4", items[0].Title);
    }

    [Fact]
    public async Task UpdateAsync_MustUpdateBook()
    {
        // Arrange
        var options = TestDbContext.CreateNewContextOptions();
        await using var context = new ApplicationDbContext(options);
        var repo = CreateRepository(context);
        var book = new Book("Livro1", "1234567890", "Autor", "Sinopse", 1, 1, 1);
        await repo.AddAsync(book);
        await repo.SaveChangesAsync();
        book.Title = "Livro Atualizado";

        // Act
        await repo.UpdateAsync(book);
        await repo.SaveChangesAsync();

        // Assert
        var updated = await context.Set<Book>().FirstOrDefaultAsync();
        Assert.Equal("Livro Atualizado", updated!.Title);
    }

    [Fact]
    public async Task DeleteAsync_MustRemoveBook()
    {
        // Arrange
        var options = TestDbContext.CreateNewContextOptions();
        await using var context = new ApplicationDbContext(options);
        var repo = CreateRepository(context);
        var book = new Book("Livro1", "1234567890", "Autor", "Sinopse", 1, 1, 1);
        await repo.AddAsync(book);
        await repo.SaveChangesAsync();

        // Act
        await repo.DeleteAsync(book);
        await repo.SaveChangesAsync();

        // Assert
        var all = await context.Set<Book>().ToListAsync();
        Assert.Empty(all);
    }
}