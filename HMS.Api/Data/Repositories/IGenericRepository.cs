using System.Linq.Expressions;

namespace HMS.Api.Data.Repositories;

public interface IGenericRepository<T> where T : class
{
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<T?> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

    void Update(T entity);

    void Delete(T entity);
}
