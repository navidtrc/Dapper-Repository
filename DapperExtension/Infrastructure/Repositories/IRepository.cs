using System.Linq.Expressions;

namespace DapperExtension.Infrastructure.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAsync(string? query);
    Task<IEnumerable<TEntity>> GetAsync<TProperty>(string? query, params Expression<Func<TEntity, TProperty>>[] referenceProperty);

    Task<TEntity> GetByIdAsync(int id, string? query);
    Task<IEnumerable<TEntity>> GetByIdAsync<TProperty>(int id, string? query, params Expression<Func<TEntity, TProperty>>[] referenceProperty);

    Task<int> AddAsync(TEntity entity);

    Task<int> UpdateAsync(TEntity entity);

    Task<bool> DeleteAsync(TEntity entity);

    Task<object> SqlRaw(string sqlQuery, TEntity entity);
    Task<object> SqlRaw(string sqlQuery, object input);
}
