using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SportsWatcher.WebApi.Interfaces;

namespace SportsWatcher.WebApi.Data;
public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    public GenericRepository(IUnitOfWork uow)
    {
        UnitOfWork = uow ?? throw new ArgumentNullException("ERROR: Null unit of work instance");
        Context = UnitOfWork.GetContext();
    }

    protected IUnitOfWork UnitOfWork { get; }
    protected DbContext Context { get; }

    public virtual ICollection<TEntity> GetAll(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        int? take = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
    {
        var query = Context.Set<TEntity>().AsQueryable();
        return PrepareQuery(query, predicate, include, orderBy, take).ToList();
    }

    public virtual async Task<ICollection<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        int? take = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
    {
        var query = Context.Set<TEntity>().AsQueryable().AsSplitQuery();
        return await PrepareQuery(query, predicate, include, orderBy, take).ToListAsync();
    }

    public virtual TEntity First(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
    {
        var query = Context.Set<TEntity>().AsQueryable();
        return PrepareQuery(query, predicate, include).First();
    }

    public virtual async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        int? take = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
    {
        var query = Context.Set<TEntity>().AsQueryable();
        return await PrepareQuery(query, predicate, include).FirstAsync();
    }

    public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
    {
        var query = Context.Set<TEntity>().AsQueryable();
        return PrepareQuery(query, predicate, include, orderBy).FirstOrDefault();
    }

    public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        int? take = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
    {
        var query = Context.Set<TEntity>().AsQueryable();
        return await PrepareQuery(query, predicate, include, orderBy).FirstOrDefaultAsync();
    }

    public virtual TEntity Single(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
    {
        var query = Context.Set<TEntity>().AsQueryable();
        return PrepareQuery(query, predicate, include).Single();
    }

    public virtual async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
    {
        var query = Context.Set<TEntity>().AsQueryable();
        return await PrepareQuery(query, predicate, include).SingleAsync();
    }

    public virtual TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
    {
        var query = Context.Set<TEntity>().AsQueryable();
        return PrepareQuery(query, predicate, include).SingleOrDefault();
    }

    public virtual async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
    {
        var query = Context.Set<TEntity>().AsQueryable();
        return await PrepareQuery(query, predicate, include).SingleOrDefaultAsync();
    }

    public virtual int Count(Expression<Func<TEntity, bool>> predicate = null)
    {
        var query = Context.Set<TEntity>().AsQueryable().AsNoTracking();
        return PrepareQuery(query, predicate).Count();
    }

    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
    {
        var query = Context.Set<TEntity>().AsQueryable().AsNoTracking();
        return await PrepareQuery(query, predicate, include).CountAsync();
    }

    public virtual TEntity Add(TEntity entity)
    {
        Context.Add(entity);
        return entity;
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        await Context.AddAsync(entity);
        return entity;
    }

    public virtual ICollection<TEntity> AddRange(ICollection<TEntity> entities)
    {
        Context.AddRange(entities);
        return entities;
    }

    public virtual async Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities)
    {
        await Context.AddRangeAsync(entities);
        return entities;
    }

    public virtual TEntity Update(TEntity entity)
    {
        Context.Update(entity);
        return entity;
    }

    public virtual ICollection<TEntity> UpdateRange(ICollection<TEntity> entities)
    {
        Context.UpdateRange(entities);
        return entities;
    }

    public virtual TEntity Remove(TEntity entity)
    {
        Context.Remove(entity);
        return entity;
    }

    public virtual ICollection<TEntity> RemoveRange(ICollection<TEntity> entities)
    {
        Context.RemoveRange(entities);
        return entities;
    }

    protected IQueryable<TEntity> PrepareQuery(
        IQueryable<TEntity> query,
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        int? take = null)
    {
        if (include != null) query = include(query);

        if (predicate != null) query = query.Where(predicate);

        if (orderBy != null) query = orderBy(query);

        if (take != null) query = query.Take(Convert.ToInt32(take));

        return query;
    }
}