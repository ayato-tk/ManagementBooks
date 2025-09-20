using System.Linq.Expressions;

namespace GestaoLivros.Domain.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id, Func<IQueryable<T>, IQueryable<T>>? queryModifier = null);
    Task<T?> GetByIdAsync(long id);
    Task<List<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>>? queryModifier = null);
    Task<(List<T> Items, int TotalCount)> GetPaginatedAsync(int page, int pageSize, string? search = null,
        Func<IQueryable<T>, IQueryable<T>>? queryModifier = null,
        params Expression<Func<T, string>>[]? searchableProperties);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task SaveChangesAsync();
}