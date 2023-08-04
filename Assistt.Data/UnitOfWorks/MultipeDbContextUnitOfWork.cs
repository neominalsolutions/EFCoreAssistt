using Asisstt.Core.DataContracts;
using Assistt.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistt.Data.UnitOfWorks
{
    public class MultipeDbContextUnitOfWork : IMultipleContextUnitOfWork
    {
        private AssisttDbContext AssisttDbContext;
        private AssisttIdentityDbContext AssisttIdentityDbContext;

        public MultipeDbContextUnitOfWork(AssisttDbContext assisttDbContext, AssisttIdentityDbContext assisttIdentityDbContext)
        {
            AssisttDbContext = assisttDbContext;
            AssisttIdentityDbContext = assisttIdentityDbContext;
        }

        public int Commit()
        {
            using (var tra1 = AssisttIdentityDbContext.Database.BeginTransaction())
            using (var tra2 = AssisttDbContext.Database.BeginTransaction())
            {
                try
                {


                    int r1 = AssisttDbContext.SaveChanges(); // 3 entries
                    int r2 = AssisttIdentityDbContext.SaveChanges(); // 5 entries

                    tra1.Commit();
                    tra2.Commit();


                    return r1 + r2;

                }
                catch (Exception)
                {
                    tra1.Rollback();
                    tra2.Rollback();
                    return 0;
                }
            }


        }

        public Task<int> CommitAsync()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
