using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using CitizenFileManagement.Core.Application.Interfaces;
using CitizenFileManagement.Infrastructure.Persistence.DataAccess;

namespace CitizenFileManagement.Infrastructure.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly CitizenFileDB _context;
    private readonly DbSet<T> _dbSet;

    public Repository(CitizenFileDB context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    // Retrieve all entities, optionally filtered, with optional eager loading
    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
    {
        IQueryable<T> query = _dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }
        return await query.ToListAsync();
    }

    // Retrieve all entities with eager loading (navigational properties)
    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, params string[]? includes)
    {
        IQueryable<T> query = _dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }
        if (includes != null)
        {
            foreach (var includeProperty in includes)
            {
                query = query.Include(includeProperty);
            }
        }
        return await query.ToListAsync();
    }

    // Retrieve a single entity with optional eager loading
    public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, params string[]? includes)
    {
        IQueryable<T> query = _dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }
        if (includes != null)
        {
            foreach (var includeProperty in includes)
            {
                query = query.Include(includeProperty);
            }
        }
        return await query.FirstOrDefaultAsync();
    }

    // Add a new entity
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    // Update an existing entity
    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    // Soft delete an entity (assuming it has an 'IsDeleted' flag)
    public void SoftDelete(T entity)
    {
        var softDeleteProperty = typeof(T).GetProperty("IsDeleted");
        if (softDeleteProperty != null)
        {
            softDeleteProperty.SetValue(entity, true);
            Update(entity);
        }
        else
        {
            throw new InvalidOperationException("Entity does not have an 'IsDeleted' property.");
        }
    }

    // Hard delete an entity
    public void HardDelete(T entity)
    {
        _dbSet.Remove(entity);
    }

    // Save changes to the database
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}