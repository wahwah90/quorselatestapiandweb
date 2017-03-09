using Quorse.AppApi.DAL.Interfaces.Base;
using Quorse.EntityFramework.EntityFramework.Schema;
using Quorse.EntityFramework.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quorse.AppApi.DAL.Interfaces
{
    public interface ICourseRepository:IBaseRepository<course,int>
    {
        IQueryable<course> GetAll();
        Task<course> GetByKeyAsync(int id);
        Task<int> SaveAsync(course course); 
        Task<course> DeleteAsync(int key);
        IQueryable<RecommendedViewModel> GetTimeTickerRecommended(int start, int size, int range);
        Task<List<RecommendedViewModel>> GetLatestCourseAsync(int start, int size, int range);
        Task<List<RecommendedViewModel>> GetPopularRecommendedAsync(int start, int size, int range);
        Task<List<RecommendedViewModel>> GetPopularCourseAsync(int start, int size, int range);
        Task<List<RecommendedViewModel>> GetSuggestionCourseAsync(int start, int size, int range, int? courseLevel1, int? courseLevel2, int? courseLevel3, int? courseLevel4);
    }
}
