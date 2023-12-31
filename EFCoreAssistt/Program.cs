using Asisstt.Core.DataContracts;
using Asisstt.Core.Repositories;
using Assistt.Data.DbContexts;
using Assistt.Data.Repositories;
using Assistt.Data.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<AssisttDbContext>(opt =>
{
  opt.UseSqlServer(builder.Configuration.GetConnectionString("AssisttConn"));
});

builder.Services.AddDbContext<AssisttIdentityDbContext>(opt =>
{
  opt.UseSqlServer(builder.Configuration.GetConnectionString("AssisttConn"));
});


builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IAssisttUnitOfWork, AssisttUnitOfWork>();
builder.Services.AddScoped<IMultipleContextUnitOfWork, MultipeDbContextUnitOfWork>();



var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
