using Asisstt.Core.DataContracts;
using Asisstt.Core.Models;
using Asisstt.Core.Repositories;
using Assistt.Data.DbContexts;
using Assistt.Data.Identity;
using Assistt.Data.UnitOfWorks;
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
    private readonly IAssisttUnitOfWork asistUnitOfWork;
    private readonly IMultipleContextUnitOfWork multiContextUnitOfWork;
    private readonly ICategoryRepository categoryRepository;
    private readonly AssisttIdentityDbContext identityDb;
    public ProductsController(AssisttDbContext db, IProductRepository productRepository, IAssisttUnitOfWork asistUnitOfWork, ICategoryRepository categoryRepository, AssisttIdentityDbContext assisttIdentityDbContext, IMultipleContextUnitOfWork multipleContextUnitOfWork)
    {
      this.db = db;
      this.productRepository = productRepository;
      this.asistUnitOfWork = asistUnitOfWork;
      this.categoryRepository = categoryRepository;
      this.identityDb = assisttIdentityDbContext;
      this.multiContextUnitOfWork = multipleContextUnitOfWork;
    }


    // ab -n 50 http://localhost:5001/api/products/paralel-list & ab -n 50 http://localhost:5001/api/products/async-list & ab -n 50 http://localhost:5001/api/products/sync-list

    [HttpGet("async-list")]
    public async Task<IActionResult> ListAsync()
    {

        var task1 = await productRepository.Query().ToListAsync();
        var task2 = await categoryRepository.Query().ToListAsync();

        return Ok();
   
    }



    /*
     * SELECT TOP(1) [p].[Id], [p].[CategoryId], [p].[Deleted], [p].[DeletedAt], [p].[DeletedBy], [p].[ProductName], [p].[Price], [p].[Stock]
FROM [Product] AS [p]
WHERE [p].[ProductName] LIKE N'%a%'
     */

    /*
     
    SELECT TOP(1) [p].[Id], [p].[CategoryId], [p].[Deleted], [p].[DeletedAt], [p].[DeletedBy], [p].[ProductName], [p].[Price], [p].[Stock]
FROM [Product] AS [p]
WHERE [p].[ProductName] LIKE N'%a%'
     */

    [HttpGet("paralel-list")]
    public async Task<IActionResult> ParalelListAsync()
    {
      using (var context = new AssisttDbContext())
      using (var context1 = new AssisttDbContext())
      {
        // tek kayıt çekerken önce FirstOrDefault() çoklu kayıt çekerken Where ifadesi kullanalım.
        var c1 = context.Products.FirstOrDefault(x => x.Name.Contains("a"));
        var c2 = context.Products.Where(x => x.Name.Contains("a")).FirstOrDefault();

        //var r1 = context.Products.Where(x => x.Deleted).OrderBy(x => x.Deleted).ToList();
        //var r2 = context.Products.ToList().Where(x => x.Deleted).OrderBy(x => x.Deleted);

      }

      // tek context instance ile birden falza async operasyonu (readOnly) işlemlerinde paralel olarak tüm sorguları işlemiyoruz. Bunu yapmak için her dbContext request birbirinden bağımsız olması lazım.

      //var task1 = productRepository.Query().ToListAsync();
      //var task2 = categoryRepository.Query().ToListAsync();

      //await Task.WhenAll(task1, task2);

      //return Ok();

      // her bir task için yeni dbContext instance açarak soruguları parelelde çalıştırabiliriz.
      using (var context = new AssisttDbContext())
      using (var context1 = new AssisttDbContext())
      using (var identityContext = new AssisttIdentityDbContext())
      {
        var task3 = identityContext.Roles.ToList();

        var task1 = context1.Products.ToListAsync();
        var task2 = context.Categories.ToListAsync();

        await Task.WhenAny(task1, task2);



        return Ok();
      }

    }


    [HttpGet("sync-list")]
    public IActionResult List()
    {

      
      var result = productRepository.Query().IgnoreQueryFilters().ToList();
      var result2 = categoryRepository.Query().ToList();

  

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

        await  asistUnitOfWork.CommitAsync();
       
     

      return Ok();
    }

    [HttpDelete]
    public IActionResult Delete()
    {
      // aşağıdaki sorgu merkezi olarak yönetilebilir olmalı.
      var plist = productRepository.Where(x => x.Deleted == false).ToList();

      productRepository.Delete("0b1cf7cd-8f51-4f59-8933-2f04d566373f");
      asistUnitOfWork.Commit();

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
  
      int affectedRows = asistUnitOfWork.Commit();




      return Ok();
    }

    [HttpGet("multipleUnitOfWork")]
    public IActionResult MultipleContextUnitOfWork()
    {
      var role = new AppRole(); // IdentityContext
      role.Id = Guid.NewGuid().ToString();
      role.Name = "Manager";

      identityDb.Add(role); 

      var c = new Category(); // AppContext
      c.Name = "LKategori1";
      categoryRepository.Create(c);



      this.multiContextUnitOfWork.Commit();

      return Ok();
    }

  }
}
