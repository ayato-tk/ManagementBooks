using GestaoLivros.Domain.Entities;
using GestaoLivros.Infra.Data.Context;
using GestaoLivros.Infra.Data.Repositories;

namespace GestaoLivros.Infra.Data.Tests.Repositories;

public class UserRepositoryTests
{
    
    private static UserRepository CreateRepository(ApplicationDbContext context) => new(context);
    
    [Fact]
    public async Task GetByEmailAsync_ShouldReturnUser_WhenEmailExists()
    {
        // Arrange
        var options = TestDbContext.CreateNewContextOptions();
        await using var context = new ApplicationDbContext(options);
        var repo = CreateRepository(context);
        const string email = "test@gmail.com";
        var userData = new User()
        {
            Name = "test",
            Email = email
        };
        await repo.AddAsync(userData);
        await repo.SaveChangesAsync();
        
        // Act
        var user = await repo.GetByEmailAsync(email);
        
        // Assert
        Assert.NotNull(user);
    }
}