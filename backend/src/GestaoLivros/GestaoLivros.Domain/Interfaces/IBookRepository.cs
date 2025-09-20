using GestaoLivros.Domain.Entities;

namespace GestaoLivros.Domain.Interfaces;

public interface IBookRepository : IBaseRepository<Book>
{
    Task<bool> ExistsByGenreIdAsync(int genreId);

    Task<bool> ExistsByPublisherIdAsync(int publisherId);
}