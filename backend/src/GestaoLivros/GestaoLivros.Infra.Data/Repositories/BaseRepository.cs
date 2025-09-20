using System.Linq.Expressions;
using GestaoLivros.Domain.Interfaces;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace GestaoLivros.Infra.Data.Repositories;

public class BaseRepository<T>(DbContext context) : IBaseRepository<T>
    where T : class
{
    private readonly DbContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public Task<T?> GetByIdAsync(int id, Func<IQueryable<T>, IQueryable<T>>? queryModifier = null)
    {
        IQueryable<T> query = _dbSet;

        if (queryModifier != null)
            query = queryModifier(query);


        return query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
    }

    public async Task<T?> GetByIdAsync(long id)
        => await _dbSet.FindAsync(id);

    public Task<List<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>>? queryModifier = null)
    { 
        IQueryable<T> query = _dbSet;

        if (queryModifier != null)
            query = queryModifier(query);
        
        return query.ToListAsync();
    }

    public async Task<(List<T> Items, int TotalCount)> GetPaginatedAsync(int page, int pageSize, string? search = null,
        Func<IQueryable<T>, IQueryable<T>>? queryModifier = null,
        params Expression<Func<T, string>>[]? searchableProperties)
    {
        IQueryable<T> query = _dbSet;

        if (queryModifier != null)
            query = queryModifier(query);

        var any = searchableProperties?.Any();

        if (!string.IsNullOrEmpty(search) && (searchableProperties?.Any() ?? false))
        {
            var predicate = PredicateBuilder.New<T>(false);

            var likeMethod = typeof(DbFunctionsExtensions).GetMethod(
                nameof(DbFunctionsExtensions.Like),
                new[] { typeof(DbFunctions), typeof(string), typeof(string) }
            ) ?? throw new InvalidOperationException("EF.Functions.Like method not found");

            foreach (var prop in searchableProperties!)
            {
                var parameter = prop.Parameters[0];

                var likeExpr = Expression.Lambda<Func<T, bool>>(
                    Expression.Call(
                        likeMethod,
                        Expression.Constant(EF.Functions),
                        prop.Body,
                        Expression.Constant($"%{search}%")
                    ),
                    parameter
                );

                predicate = predicate.Or(likeExpr);
            }

            query = query.Where(predicate);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(x => EF.Property<int>(x, "Id"))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task AddAsync(T entity)
        => await _dbSet.AddAsync(entity);

    public Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
}