using Quorse.AppApi.BL.Interfaces;
using Quorse.AppApi.DAL.Interfaces;
using Quorse.AppApi.DAL.Repositories;
using Quorse.EntityFramework.EntityFramework.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quorse.AppApi.BL.Services
{
    public class EntityService : IEntityService
    {
        private readonly IEntityRepository EntityRepository;
        public EntityService(IEntityRepository entityRepository)
        {
            if (entityRepository == null)
                throw new ArgumentNullException();

            EntityRepository = entityRepository;
        }
        public EntityService()
        {
            EntityRepository = new EntityRepository();
        }
        public async Task<entity> GetEntityByGuidAsync(string guid)
        {
            return await EntityRepository.GetEntityByGuidAsync(guid);
        }
    }
}
