﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asisstt.Core.Models
{
  public class Category:BaseEntity<int>
  {
    public string Name { get; set; }
    public string? Description { get; set; }
    // public List<Product>? Products { get; set; }

  }
}
