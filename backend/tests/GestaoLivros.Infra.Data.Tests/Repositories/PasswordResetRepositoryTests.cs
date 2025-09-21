using GestaoLivros.Domain.Entities;
using GestaoLivros.Infra.Data.Context;
using GestaoLivros.Infra.Data.Repositories;

namespace GestaoLivros.Infra.Data.Tests.Repositories;

public class PasswordResetRepositoryTests
{
    private static PasswordResetRepository CreateRepository(ApplicationDbContext context) => new(context);
    
    [Fact]
    public async Task GetByTokenAsync_ShouldReturnPasswordResetToken_WhenTokenIsValid()
    {
        // Arrange
        var options = TestDbContext.CreateNewContextOptions();
        await using var context = new ApplicationDbContext(options);
        var repo = CreateRepository(context);
        const string passwordResetToken = "test";
        var passwordResetTokenData = new PasswordResetToken()
        {
            Token = passwordResetToken
        };
        await repo.AddAsync(passwordResetTokenData);
        await repo.SaveChangesAsync();
        
        // Act
        var exists = await repo.GetByTokenAsync(passwordResetToken);
        
        // Assert
        Assert.NotNull(exists);
        Assert.Equal(passwordResetToken, exists.Token);
    }
}