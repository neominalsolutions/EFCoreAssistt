using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asisstt.Core.Models
{
  public interface IDeleteEntity
  {
    bool Deleted { get; set; }
    DateTime? DeletedAt { get; set; }
    public string DeletedBy { get; set; }

  }
}
