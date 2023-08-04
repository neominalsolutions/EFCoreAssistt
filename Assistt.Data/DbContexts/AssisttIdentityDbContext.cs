using Assistt.Data.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistt.Data.DbContexts
{
  public class AssisttIdentityDbContext:IdentityDbContext<AppUser,AppRole,string>
  {
    public AssisttIdentityDbContext()
    {

    }
    public AssisttIdentityDbContext(DbContextOptions<AssisttIdentityDbContext> opt):base(opt)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer("Server=(localDB)\\MyLocalDb;Database=EFCoreDB1;Trusted_Connection=True;MultipleActiveResultSets=True");

      base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      

      base.OnModelCreating(builder);

      builder.Entity<AppUser>().ToTable("User", "Identity");
    }
  }
}
