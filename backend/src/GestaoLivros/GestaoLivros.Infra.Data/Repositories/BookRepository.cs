using GestaoLivros.Domain.Entities;
using GestaoLivros.Domain.Interfaces;
using GestaoLivros.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace GestaoLivros.Infra.Data.Repositories;

public class BookRepository(ApplicationDbContext context) : BaseRepository<Book>(context), IBookRepository
{
    public async Task<bool> ExistsByGenreIdAsync(int genreId)
    {
        return await _dbSet.AnyAsync(b => b.GenreId == genreId);
    }
    public async Task<bool> ExistsByPublisherIdAsync(int publisherId)
    {
        return await _dbSet.AnyAsync(b => b.PublisherId == publisherId);
    }
    
}