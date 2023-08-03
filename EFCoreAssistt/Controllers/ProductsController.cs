﻿using Asisstt.Core.DataContracts;
using Asisstt.Core.Models;
using Asisstt.Core.Repositories;
using Assistt.Data.DbContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EFCoreAssistt.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProductsController : ControllerBase
  {
    private readonly AssisttDbContext db;
    private readonly IProductRepository productRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly ICategoryRepository categoryRepository;
    public ProductsController(AssisttDbContext db, IProductRepository productRepository, IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
    {
      this.db = db;
      this.productRepository = productRepository;
      this.unitOfWork = unitOfWork;
      this.categoryRepository = categoryRepository;
    }


    // ab -n 50 http://localhost:5001/api/products/async-list

    [HttpGet("async-list")]
    public async Task<IActionResult> ListAsync()
    {

        var task1 = await productRepository.ListAsync();
        var task2 = await categoryRepository.ListAsync();

        return Ok();
   
    }

    [HttpGet("paralel-list")]
    public async Task<IActionResult> ParalelListAsync()
    {

      using (var context = new AssisttDbContext())
      using (var context1 = new AssisttDbContext()) { 
        var task1 = context1.Products.ToListAsync();
        var task2 = context.Categories.ToListAsync();

        await Task.WhenAll(task1, task2);

       

        return Ok();
      }
    
    }


    [HttpGet("sync-list")]
    public IActionResult List()
    {

      
      var result = productRepository.List();
      var result2 = categoryRepository.List();

  

      return Ok();
    }


    [HttpGet("async")]
    public  async Task<IActionResult> CreateAsync()
    {

      var random = new Random();

     
        var p = new Product();
        p.Name = Guid.NewGuid().ToString();
        p.SetPrice(random.Next(0, 100));
        p.SetStock(random.Next(0, 100));

        var c = new Category();
        c.Name = Guid.NewGuid().ToString();

        await productRepository.CreateAsync(p);
        await categoryRepository.CreateAsync(c);

        // son işlemi veri tabanına yansıtmak için burada await kullandık

        await  unitOfWork.CommitAsync();

      return Ok();
    }



    [HttpGet("sync")]
    public IActionResult CreateSenkron() 
    {
    
      var random = new Random();

        var p = new Product();
        p.Name = Guid.NewGuid().ToString();
        p.SetPrice(random.Next(0, 100));
        p.SetStock(random.Next(0, 100));

        var c = new Category();
        c.Name = Guid.NewGuid().ToString();

        productRepository.Create(p);
        categoryRepository.Create(c);
  
      int affectedRows = unitOfWork.Commit();




      return Ok();
    }

  }
}
