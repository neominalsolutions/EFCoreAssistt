using Asisstt.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Asisstt.Core.DataContracts
{
  public interface IRepository<TEntity,TKey>
    where TEntity:BaseEntity<TKey> 
    where TKey:IComparable<TKey>
  {
    void Create(TEntity entity);
    void Update(TEntity entity);
    void Delete(TKey key);

    TEntity FindById(TKey key);

    TEntity Find(Expression<Func<TEntity, bool>> lambda);

    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> lambda);

    IEnumerable<TEntity> List();

  }
}
