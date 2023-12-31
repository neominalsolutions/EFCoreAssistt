﻿using Asisstt.Core.DataContracts;
using Assistt.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistt.Data.UnitOfWorks
{
    public class AssisttUnitOfWork : IAssisttUnitOfWork
    {
        private AssisttDbContext context;

        public AssisttUnitOfWork(AssisttDbContext context)
        {
            this.context = context;
        }

        public int Commit()
        {

            //using (var tran = context.Database.BeginTransaction())
            //{
            //  try
            //  {
            //    int result = this.context.SaveChanges();
            //    tran.Commit();
            //    return result;
            //  }
            //  catch (Exception)
            //  {
            //    tran.Rollback();
            //    return 0;
            //  }
            //}


            try
            {
                return context.SaveChanges(); // auto-Commit yöntemi
            }
            catch (Exception)
            {
                return 0;
            }


        }

        public Task<int> CommitAsync()
        {
            try
            {
                return context.SaveChangesAsync(); // auto-Commit yöntemi
            }
            catch (Exception)
            {
                return Task.FromResult(0);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
