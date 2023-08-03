using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asisstt.Core.DataContracts
{
  public interface IUnitOfWork:IDisposable
  {


    int Commit(); // resultsets affected row etkilenen kayıt sayısı

    Task<int> CommitAsync();
  }

   
}
