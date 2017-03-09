using Quorse.AppApi.DAL.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quorse.EntityFramework.EntityFramework.Schema;
using Quorse.EntityFramework.ViewModels;

namespace Quorse.AppApi.DAL.Interfaces
{
    public interface IClassDetailRepository : IBaseRepository<classDetail, int>
    {
        Task<List<ClassComplexModel>> GetClassesAsync(int courseId);
        Task<bool> HasTypeOfClassAsync(int courseid, int type);
        Task<DateTime?> GetTTEndTime(int courseid, int range);
    }
}
