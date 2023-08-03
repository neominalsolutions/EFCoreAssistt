using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asisstt.Core.Models
{
  // api/products/545454-1545454-4545212
  public abstract class BaseEntity<TKey> where TKey: IComparable<TKey>
  {
    public TKey Id { get; init; }
  }
}
