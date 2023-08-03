using Asisstt.Core.DataContracts;
using Asisstt.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asisstt.Core.Repositories
{
  public interface IProductRepository:IRepository<Product,string>,IAsyncRepository<Product,string>
  {
    // sadece IProductRepoya özgü bir yetenek
    IEnumerable<Product> IncludeWithCategory();
  }
}
