using GestaoLivros.Domain.Entities;
using GestaoLivros.Domain.Interfaces;
using GestaoLivros.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace GestaoLivros.Infra.Data.Repositories;

public class PasswordResetRepository(ApplicationDbContext context)
    : BaseRepository<PasswordResetToken>(context), IPasswordResetRepository
{
    public Task<PasswordResetToken?> GetByTokenAsync(string token)
        => _dbSet.FirstOrDefaultAsync(t => t.Token == token);
}