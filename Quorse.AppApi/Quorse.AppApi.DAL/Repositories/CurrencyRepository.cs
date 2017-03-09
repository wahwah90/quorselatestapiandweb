using Quorse.AppApi.DAL.Interfaces;
using Quorse.AppApi.DAL.Repositories.Base;
using Quorse.EntityFramework.EntityFramework.Schema;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quorse.AppApi.DAL.Repositories
{
    public class CurrencyRepository:BaseFuncRepository,ICurrencyRepository
    {
        private QuorseDbEntities db = new QuorseDbEntities();

        public Task<currency> DeleteAsync(int key)
        {
            throw new NotImplementedException();
        }

        public IQueryable<currency> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<currency> GetByKeyAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveAsync(currency entry)
        {
            throw new NotImplementedException();
        }
        public async Task<currency> GetLatestCurrencyAsync()
        {
            return await db.currencies.OrderByDescending(c => c.CurrencyID).FirstOrDefaultAsync();
        }
    }
}
