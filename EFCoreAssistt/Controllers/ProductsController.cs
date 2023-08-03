using Asisstt.Core.DataContracts;
using Asisstt.Core.Models;
using Asisstt.Core.Repositories;
using Assistt.Data.DbContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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

    // abs -n 10 http://localhost:5001/api/products/sync-list & abs -n 10 http://localhost:5001/api/products/async-list
    // 5.501


    [HttpGet("async-list")]
    public async Task<IActionResult> CreateListAsync()
    {


      var result =   productRepository.Where().ToListAsync();
      var result2 =  categoryRepository.Where().ToListAsync();

      await Task.WhenAll(result, result2);


      return Ok();
    }

    [HttpGet("async-list-await")]
    public async Task<IActionResult> CreateListAwaitAsync()
    {


      var result = await productRepository.Where().ToListAsync();
      var result2 = await categoryRepository.Where().ToListAsync();


      return Ok();
    }


    [HttpGet("sync-list")]
    public async Task<IActionResult> CreateListsync()
    {

      
      var result = productRepository.Where().ToList();
      var result2 =  categoryRepository.Where().ToList();

      return Ok();
    }

    //ab -n 50 -c 50 http://localhost:5001/api/products/sync & ab -n 50 -c 50 http://localhost:5001/api/products/async

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
