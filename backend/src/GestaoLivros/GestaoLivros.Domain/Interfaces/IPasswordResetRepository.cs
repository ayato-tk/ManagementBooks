using GestaoLivros.Domain.Entities;

namespace GestaoLivros.Domain.Interfaces;

public interface IPasswordResetRepository : IBaseRepository<PasswordResetToken>
{
    Task<PasswordResetToken?> GetByTokenAsync(string token);
}