using Quorse.AppApi.BL.Interfaces.Base;
using Quorse.EntityFramework.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quorse.AppApi.BL.Interfaces
{
    public interface IClassDetailService : IBaseService
    {
        Task<List<ClassComplexModel>> GetClassesAsync(int courseId);
        Task<bool> HasTypeOfClassAsync(int courseid, int type);
        Task<DateTime?> GetTTEndTime(int courseid, int range);
    }
}
