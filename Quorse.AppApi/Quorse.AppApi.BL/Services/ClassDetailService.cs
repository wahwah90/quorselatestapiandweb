using Quorse.AppApi.BL.Interfaces;
using Quorse.AppApi.DAL.Interfaces;
using Quorse.AppApi.DAL.Repositories;
using Quorse.EntityFramework.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quorse.AppApi.BL.Services
{
    public class ClassDetailService : IClassDetailService
    {
        private readonly IClassDetailRepository ClassDetailRepository;
        public ClassDetailService(IClassDetailRepository classDetailRepository)
        {
            if (classDetailRepository == null)
                throw new ArgumentNullException();

            ClassDetailRepository = classDetailRepository;
        }
        public ClassDetailService()
        {
            ClassDetailRepository = new ClassDetailRepository();
        }
        public async Task<List<ClassComplexModel>> GetClassesAsync(int courseId)
        {
            return await ClassDetailRepository.GetClassesAsync(courseId);
        }

        public async Task<bool> HasTypeOfClassAsync(int courseid, int type)
        {
            return await ClassDetailRepository.HasTypeOfClassAsync(courseid,type);
        }
        public async Task<DateTime?> GetTTEndTime(int courseid, int range)
        {
            return await ClassDetailRepository.GetTTEndTime(courseid, range);
        }

    }
}