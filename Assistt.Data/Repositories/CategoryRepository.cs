using Asisstt.Core.DataContracts;
using Asisstt.Core.Models;
using Asisstt.Core.Repositories;
using Assistt.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistt.Data.Repositories
{
  public sealed class CategoryRepository : EFBaseRepository<AssisttDbContext, Category, int>, ICategoryRepository
  {
    public CategoryRepository(AssisttDbContext context) : base(context)
    {
    }
  }
}
