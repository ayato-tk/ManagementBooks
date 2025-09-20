using GestaoLivros.Domain.Entities;
using GestaoLivros.Domain.Interfaces;
using GestaoLivros.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace GestaoLivros.Infra.Data.Repositories;

public class UserRepository(ApplicationDbContext context) : BaseRepository<User>(context), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet.Where(u => u.Email == email).FirstOrDefaultAsync();
    }
}