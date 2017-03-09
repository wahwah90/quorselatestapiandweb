using Quorse.AppApi.DAL.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quorse.EntityFramework.EntityFramework.Schema;

namespace Quorse.AppApi.DAL.Interfaces
{
    public interface IEntityRepository : IBaseRepository<entity, int>
    {
        Task<entity> GetEntityByGuidAsync(string guid);
    }
}
