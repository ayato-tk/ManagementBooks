namespace GestaoLivros.Application.Services.Interfaces;

public interface ITokenService
{
    string GenerateToken(string userId);
}