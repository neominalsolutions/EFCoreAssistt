using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asisstt.Core.Models
{
  public abstract class BaseDeletedEntity<TKey> : BaseEntity<TKey>, IDeleteEntity
    where TKey:IComparable<TKey>
  {
    public bool Deleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
  }
}
