using System.Linq.Expressions;

namespace CitizenFileManagement.Core.Application.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, params string[]? includes);
    Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, params string[]? includes);
    Task AddAsync(T entity);
    void Update(T entity);
    void SoftDelete(T entity);
    void HardDelete(T entity);
    Task SaveAsync();
}