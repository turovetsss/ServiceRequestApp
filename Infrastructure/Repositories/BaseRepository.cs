using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repositories;

public abstract class RepositoryBase<T>(ApplicationDbContext context) : IRepository<T> 
    where T : class
{
    private IRepository<T> _repositoryImplementation;

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await context.Set<T>().FindAsync(id);
    }
    public virtual async Task<T?> GetByIdWithIncludesAsync(
        int id, 
        params Expression<Func<T, object>>[] includes)
    {
        var query = context.Set<T>().AsQueryable();
        
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await context.Set<T>().ToListAsync();
    }

    public abstract Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    public abstract Task AddAsync(T entity);
    public abstract void Update(T entity);
    public abstract void Delete(T entity);
    public abstract Task SaveChangesAsync();

    public virtual async Task<IEnumerable<T>> GetWithIncludesAsync(
        params Expression<Func<T, object>>[] includes)
    {
        var query = context.Set<T>().AsQueryable();
        
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.ToListAsync();
    }

    public virtual async Task CreateAsync(T entity)
    {
        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(T entity)
    {
        context.Set<T>().Update(entity);
        await context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}