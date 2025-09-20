using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GestaoLivros.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GestaoLivros.Application.Services.Implementations;

public class TokenService(IConfiguration configuration ) : ITokenService
{

    private readonly string _secret = configuration["Authorization:Secret"] ?? throw new NullReferenceException("Jwt Secret key is not defined");
    
    public string GenerateToken(string userId)
    {
        var handler = new JwtSecurityTokenHandler();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.NameIdentifier, userId)
            ]),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        };

        var token = handler.CreateToken(descriptor);
        
        return handler.WriteToken(token);
    }
}