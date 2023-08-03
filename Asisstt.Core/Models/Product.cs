using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Asisstt.Core.Models
{
  public record Money
  {
    public int Amount { get; init; }
    public string Currency { get; init; }

    public Money(int amount,string currency)
    {
      this.Amount = amount;
      this.Currency = currency;
    }
  }

  public class Product:BaseDeletedEntity<string>
  {
    public string Name { get;  set; }
    public decimal Price { get; private set; }
    // sadece bu değer class içinden set edilebilir.
    public int Stock { get; private set; }

    public Product()
    {
      //var m1 = new Money(100, "Dolar");
      //var m2 = new Money(100, "Dolar");

      //var result = m1.Equals(m2);
      this.Id = Guid.NewGuid().ToString();
    }

    //public void SetId()
    //{
    //  this.Id = "r323432432";
    //}

    public void SetPrice(decimal price)
    {
      if (price < 0)
        price = 0;

      this.Price = price;
    }

    private int maxStockLimit = 100; 

    public void SetStock(int stock)
    {
      if (stock > maxStockLimit)
        throw new Exception("Stok değer eşiğini geçtiniz");

      if (stock < 0)
        throw new Exception("Stok değer min 1 olmalıdır");

      this.Stock = stock;
    }

    // FK
    public int? CategoryId { get; set; }

    // Navigation Property
    public Category? Category { get; set; }
  


  }
}
