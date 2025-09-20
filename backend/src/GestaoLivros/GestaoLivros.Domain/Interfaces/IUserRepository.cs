using GestaoLivros.Domain.Entities;

namespace GestaoLivros.Domain.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}