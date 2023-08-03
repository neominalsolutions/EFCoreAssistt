using Asisstt.Core.DataContracts;
using Asisstt.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asisstt.Core.Repositories
{
  public interface ICategoryRepository:IRepository<Category,int>, IAsyncRepository<Category,int>
  {

  }
}
