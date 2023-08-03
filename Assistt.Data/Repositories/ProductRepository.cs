using Asisstt.Core.DataContracts;
using Asisstt.Core.Models;
using Asisstt.Core.Repositories;
using Assistt.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistt.Data.Repositories
{
  public sealed class ProductRepository : EFBaseRepository<AssisttDbContext, Product, string>, IProductRepository
  {
    public ProductRepository(AssisttDbContext context) : base(context)
    {
    }

    public IEnumerable<Product> IncludeWithCategory()
    {
      return this.dbSet.Include(x => x.Category).AsNoTracking().ToList();
    }
  }
}
