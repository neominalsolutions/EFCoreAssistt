﻿using Asisstt.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Assistt.Data.DbContexts
{
  public class AssisttDbContext:DbContext
  {
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    public AssisttDbContext()
    {

    }

    public AssisttDbContext(DbContextOptions<AssisttDbContext> opt) : base(opt)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // FLUENT API
      modelBuilder.Entity<Product>().ToTable("Product");
      modelBuilder.Entity<Product>().Property(x => x.Name).HasColumnName("ProductName");
      //modelBuilder.Entity<Product>().Property(x => x.Name).HasColumnType("nvarchar(50)");


      base.OnModelCreating(modelBuilder);
    }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer("Server=(localDB)\\MyLocalDb;Database=EFCoreDB1;Trusted_Connection=True;MultipleActiveResultSets=True");

      base.OnConfiguring(optionsBuilder);
    }

  }
}
