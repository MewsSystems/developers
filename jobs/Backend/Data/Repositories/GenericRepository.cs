using Data.Database;
using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected ApplicationDbContext Context;
    protected DbSet<T> DbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        Context = context;
        DbSet = Context.Set<T>();
    }

    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        var entitySaved = DbSet.Add(entity);
        await Context.SaveChangesAsync(cancellationToken);
        return entitySaved.Entity;
    }

    public virtual async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        var entitySaved = DbSet.Update(entity);
        await Context.SaveChangesAsync(cancellationToken);
        return entitySaved.Entity;        
    }

    public async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.SingleOrDefaultAsync(predicate);
    }

    public T? LastElement()
    {
        var list = DbSet.OrderByDescending(e => e.CreatedDate).ToList();
        if(list.Any())
        {
            return list[0];
        }
        return null;
    }
}