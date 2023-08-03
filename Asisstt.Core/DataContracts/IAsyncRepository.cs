using Asisstt.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Asisstt.Core.DataContracts
{
  public interface IAsyncRepository<TEntity,TKey> 
    where TKey:IComparable<TKey>
    where TEntity:BaseEntity<TKey>
  {
    Task DeleteAsync(TKey key); // void e karşılık Task kullanırız
    Task CreateAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task<TEntity> FindByIdAsync(TKey key);

    Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> lambda);

    Task<IQueryable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> lambda);

    Task<IEnumerable<TEntity>> ListAsync();

  }
}
