using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GestaoLivros.Application.Services.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GestaoLivros.Application.Tests.Services;

public class TokenServiceTests
{
    [Fact]
    public void GenerateToken_ShouldReturnValidJwt_WithUserIdClaim()
    {
        // Arrange
        const string secret = "ySERWIczYXSHKtR5dTZom49nq5zXvzD4dHTX5QKnAOLd8C4EUc";
        var inMemorySettings = new Dictionary<string, string>
        {
            {"Authorization:Secret", secret}
        };
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        var service = new TokenService(configuration);
        const string userId = "42";

        // Act
        var tokenString = service.GenerateToken(userId);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(tokenString));
        
        var handler = new JwtSecurityTokenHandler();
        
        var key = Encoding.UTF8.GetBytes(secret);
        
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        var principal = handler.ValidateToken(tokenString, validationParameters, out var validatedToken);
        
        var claim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        Assert.NotNull(claim);
        Assert.Equal(userId, claim!.Value);
    }
}