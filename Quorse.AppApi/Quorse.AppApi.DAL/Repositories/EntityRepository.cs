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
    public class EntityRepository : BaseFuncRepository, IEntityRepository
    {
        private QuorseDbEntities db = new QuorseDbEntities();

        public Task<entity> DeleteAsync(int key)
        {
            throw new NotImplementedException();
        }

        public IQueryable<entity> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<entity> GetByKeyAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveAsync(entity entry)
        {
            throw new NotImplementedException();
        }
        public async Task<entity> GetEntityByGuidAsync(string guid)
        {
            return await db.entities.Where(c => c.guid == guid).FirstOrDefaultAsync();
        }
    }
}
