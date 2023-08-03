using Asisstt.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Asisstt.Core.DataContracts
{
  public abstract class EFBaseRepository<TContext, TEntity, TKey> : IRepository<TEntity, TKey>, IAsyncRepository<TEntity,TKey>
    where TContext : DbContext
    where TEntity : BaseEntity<TKey>
    where TKey : IComparable<TKey>
  {
    protected TContext context;
    protected DbSet<TEntity> dbSet;

    public EFBaseRepository(TContext context)
    {
      this.context = context;
      this.dbSet = this.context.Set<TEntity>();
    }

    public virtual void Create(TEntity entity)
    {
      //this.context.Set<TEntity>().Add(entity);
      this.dbSet.Add(entity);
    }

  

    public virtual void Delete(TKey key)
    {
     var entity = this.dbSet.Find(key);

      if (entity is null)
        throw new Exception("Entity Not Found");

      dbSet.Remove(entity); // EntityState Removed

    }

  

    public virtual TEntity Find(Expression<Func<TEntity, bool>> lambda)
    {
      var entity = this.dbSet.FirstOrDefault(lambda);

      if (entity is null)
        return default;

      return entity;
    }

  

    public virtual TEntity FindById(TKey key)
    {
      var entity = this.dbSet.Find(key);

      if (entity is null)
        throw new Exception("Entity Not Found");

      return entity;
    }


    public virtual void Update(TEntity entity)
    {
      this.dbSet.Update(entity);
    }

    public Task DeleteAsync(TKey key)
    {
      Delete(key); // senkron kodu çağırdık.
      return Task.CompletedTask;
    }

    public Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> lambda)
    {
      return this.dbSet.FirstOrDefaultAsync(lambda);
    }
    public async Task<TEntity> FindByIdAsync(TKey key)
    {
      var entity = await this.dbSet.FindAsync(key);

      if (entity is null)
        throw new Exception("Entity Not Found");

      return entity;
    }

    public  Task CreateAsync(TEntity entity)
    {
       this.dbSet.AddAsync(entity);
      return Task.CompletedTask;
    }

    public Task UpdateAsync(TEntity entity)
    {
       this.dbSet.Update(entity);
      //return Task.FromException(new Exception("Ali"));
      return Task.CompletedTask;
    }

    public virtual IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> lambda = null)
    {
      return dbSet.AsNoTracking().Where(lambda);
    }

    public async Task<IQueryable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> lambda = null)
    {
      return await Task.FromResult(dbSet.Where(lambda).AsQueryable());
    }
  }
}
