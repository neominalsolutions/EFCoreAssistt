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


    [HttpGet("async-list")]
    public async Task<IActionResult> CreateListAsync()
    {
      

     var result = (await productRepository.WhereAsync(x => x.Price > 10)).ToListAsync();


      return Ok();
    }


    [HttpGet("sync-list")]
    public async Task<IActionResult> CreateListsync()
    {


      var result = productRepository.Where(x => x.Price > 10).ToList();


      return Ok();
    }

    [HttpGet("async")]
    public async Task<IActionResult> CreateAsync()
    {
      Stopwatch s = new Stopwatch();
      var random = new Random();
      

      s.Start();

      var p = new Product();
      p.Name = Guid.NewGuid().ToString();
      // p.Price = -34;
      p.SetPrice(random.Next(0,100));
      p.SetStock(random.Next(0, 100));

      var c = new Category();
      c.Name = Guid.NewGuid().ToString();

       productRepository.CreateAsync(p);
       categoryRepository.CreateAsync(c);


      // son işlemi veri tabanına yansıtmak için burada await kullandık
      int affectedRows =  await unitOfWork.CommitAsync();

      s.Stop();

      return Ok(new {time = s.ElapsedMilliseconds});
    }


    [HttpGet("sync")]
    public IActionResult CreateSenkron() 
    {
      Stopwatch s = new Stopwatch();
      s.Start();
      var random = new Random();

      var p = new Product();
      p.Name = Guid.NewGuid().ToString();
      // p.Price = -34;
      p.SetPrice(random.Next(0, 100));
      p.SetStock(random.Next(0, 100));

      var c = new Category();
      c.Name = Guid.NewGuid().ToString();



      // abs - n 10 - c 10 http://localhost:5234/api/products/sync

      productRepository.Create(p);
      categoryRepository.Create(c);
      int affectedRows = unitOfWork.Commit();

      // http://localhost:5234/api/Products/sync

      s.Stop();

      return Ok(new { time = s.ElapsedMilliseconds });
    }

  }
}
