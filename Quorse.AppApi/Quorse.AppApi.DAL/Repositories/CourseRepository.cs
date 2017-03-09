using Quorse.AppApi.DAL.Interfaces;
using Quorse.AppApi.DAL.Repositories.Base;
using Quorse.EntityFramework.EntityFramework.Schema;
using Quorse.EntityFramework.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Quorse.AppApi.Helper.ClassDetailTypeConverter;

namespace Quorse.AppApi.DAL.Repositories
{
    public class CourseRepository : BaseFuncRepository, ICourseRepository
    {
        private QuorseDbEntities db = new QuorseDbEntities();
        public IQueryable<course> GetAll()
        {
            return db.courses;
        }

        public async Task<course> GetByKeyAsync(int id)
        {
            course course = await db.courses.FindAsync(id);
            return course;
        }

        public async Task<int> SaveAsync(course course)
        {
            if (course.id == 0)
            {
                db.courses.Add(course);
            }
            else
            {
                course dbEntry = await db.courses.FindAsync(course.id);
                if (dbEntry != null)
                {
                    dbEntry.id = course.id;
                    dbEntry.title = course.title;
                }
            }
            return await db.SaveChangesAsync();
        }
        public async Task<course> DeleteAsync(int courseID)
        {
            course dbEntry = db.courses.Find(courseID);
            if (dbEntry != null)
            {
                db.courses.Remove(dbEntry);
            }
            await db.SaveChangesAsync();
            return dbEntry;
        }

        public IQueryable<RecommendedViewModel> GetTimeTickerRecommended(int start, int size, int range)
        {

            var dateRange = DateTime.Now.AddDays(range); //show time ticker detail within date range
            var usdExchangeRate = db.currencies.OrderByDescending(c => c.CurrencyID).FirstOrDefault().CurrencyRate;
            var timeTickerCourses = db.courses.Include(c => c.classDetails)
                .Where(c => c.classDetails.Any(d => d.istimeticker == true
                                && d.status == 1
                                && DateTime.Now >= d.ttstartdate
                                && d.ttenddate >= DateTime.Now
                                && d.ttenddate < dateRange))
                .Select(c => new RecommendedViewModel
                {
                    CourseId = c.id,
                    CourseTitle = (c.title.Length > 30) ? c.title.Substring(0, 30) + "..." : c.title,
                    CourseImage = "https://quorse.s3.amazonaws.com/pub/course/course/" + c.bgLarge,
                    CourseEntityName = db.entities
                                            .Where(e => e.guid == c.entityguid)
                                            .FirstOrDefault().name,
                    CoursePrice = (c.isusd == true) ? c.courseprice * usdExchangeRate : c.courseprice,
                    CourseRating = c.courserating,
                    CourseClassTTEndDate = c.classDetails
                        .Where(d => d.istimeticker == true
                                && d.status == 1
                                && DateTime.Now >= d.ttstartdate
                                && d.ttenddate >= DateTime.Now
                                && d.ttenddate < dateRange)
                        .FirstOrDefault()
                        .ttenddate,
                    ClassSimpleModel = c.classDetails
                        .Where(d => d.istimeticker == true
                                    && d.status == 1
                                    && DateTime.Now >= d.ttstartdate
                                    && d.ttenddate >= DateTime.Now
                               )
                        .OrderBy(d => d.ttprice)
                        .Select(d => new ClassSimpleModel
                        {
                            CourseLowestClassPrice = (d.isusd == true) ? d.ttprice * usdExchangeRate : d.ttprice, //since this is time ticker,so ttprice must be lowest
                            CourseHighestClassPrice = (d.isusd == true) ? d.price * usdExchangeRate : d.price,
                            CourseDiscountRate = (d.isusd == true) ? (((d.price * usdExchangeRate) - (d.ttprice * usdExchangeRate)) / (d.price * usdExchangeRate)) * 100 : ((d.price - d.ttprice) / d.price) * 100
                        }).FirstOrDefault()
                        ,
                    CourseHasELearningClass = c.classDetails
                        .Where(d => d.istimeticker == true
                                    && d.status == 1
                                    && DateTime.Now >= d.ttstartdate
                                    && d.ttenddate >= DateTime.Now
                            && d.type == (int)ClassDetailType.ELEARNING)
                        .Count() > 0,
                    CourseHasLVCClass = c.classDetails
                        .Where(d => d.istimeticker == true
                                    && d.status == 1
                                    && DateTime.Now >= d.ttstartdate
                                    && d.ttenddate >= DateTime.Now
                                    && d.type == (int)ClassDetailType.LIVEVIRTUAL)
                        .Count() > 0,
                    CourseHasPublicClass = c.classDetails
                        .Where(d => d.istimeticker == true
                                    && d.status == 1
                                    && DateTime.Now >= d.ttstartdate
                                    && d.ttenddate >= DateTime.Now
                                    && d.type == (int)ClassDetailType.PUBLIC)
                        .Count() > 0
                })
                .OrderBy(c => c.CourseId)
                .Skip(start)
                .Take(size);

            return timeTickerCourses;

        }
        public async Task<List<RecommendedViewModel>> GetLatestCourseAsync(int start, int size, int range)
        {
            var dateRange = DateTime.Now.AddMonths(-range);
            return await db.courses.Where(c => c.CourseStatus != -1
                                               && c.CourseStatus != 3
                                               && c.CourseStatus != 4
                                               && c.publishDate>dateRange
                                               && c.classDetails.Any(d => d.classDates.Any(e => e.date > dateRange))
                                            )
                                            .Select(c => new RecommendedViewModel
                                            {
                                                CourseId = c.id,
                                                CourseTitle = (c.title.Length > 30) ? c.title.Substring(0, 30) + "..." : c.title,
                                                CourseImage = "https://quorse.s3.amazonaws.com/pub/course/course/" + c.bgLarge,
                                                CourseEntityName = c.entityguid,
                                                CoursePrice = c.courseprice,
                                                CourseRating = c.courserating,
                                                CourseIsUsd = c.isusd,
                                               
                                            })
                                            .OrderByDescending(c => c.CourseId)
                                            .Skip(start)
                                            .Take(size)
                                            .ToListAsync();
        }
        public async Task<List<RecommendedViewModel>> GetPopularCourseAsync(int start, int size, int range)
        {
            var dateRange = DateTime.Now.AddMonths(-range);
            return await db.courses.Where(c => c.CourseStatus != -1
                                               && c.CourseStatus != 3
                                               && c.CourseStatus != 4
                                               && c.courserating >= 0.1
                                               && c.courserating <= 5
                                               && c.classDetails.Any(d=>d.classDates.Any(e=>e.date> dateRange))
                                            )
                                            .Select(c=>new RecommendedViewModel{
                                                CourseId = c.id,
                                                CourseTitle = (c.title.Length > 30) ? c.title.Substring(0, 30) + "..." : c.title,
                                                CourseImage = "https://quorse.s3.amazonaws.com/pub/course/course/" + c.bgLarge,
                                                CourseEntityName = c.entityguid,
                                                CoursePrice = c.courseprice,
                                                CourseRating = c.courserating,
                                                CourseIsUsd = c.isusd
                                            })
                                            .OrderByDescending(c => c.CourseRating)
                                            .Skip(start)
                                            .Take(size)
                                            .ToListAsync();
        }

        public async Task<List<RecommendedViewModel>> GetSuggestionCourseAsync(int start, int size, int range,int? courseLevel1,int? courseLevel2,int? courseLevel3, int? courseLevel4)
        {
            var dateRange = DateTime.Now.AddMonths(-range);
            return await db.courses.Where(c =>(courseLevel4!=null&&c.courseLevel4==courseLevel4)
                                               && (courseLevel3 != null && c.courseLevel3 == courseLevel3)
                                               && (courseLevel2 != null && c.courseLevel2 == courseLevel2)
                                               && (courseLevel1 != null && c.courseLevel1 == courseLevel1)
                                               && c.CourseStatus != -1
                                               && c.CourseStatus != 3
                                               && c.CourseStatus != 4                                        
                                               && c.classDetails.Any(d => d.classDates.Any(e => e.date > dateRange))
                                            )
                                            .Select(c => new RecommendedViewModel
                                            {
                                                CourseId = c.id,
                                                CourseTitle = (c.title.Length > 30) ? c.title.Substring(0, 30) + "..." : c.title,
                                                CourseImage = "https://quorse.s3.amazonaws.com/pub/course/course/" + c.bgLarge,
                                                CourseEntityName = c.entityguid,
                                                CoursePrice = c.courseprice,
                                                CourseRating = c.courserating,
                                                CourseIsUsd = c.isusd
                                            })
                                            .OrderByDescending(c => c.CourseId)
                                            .Skip(start)
                                            .Take(size)
                                            .ToListAsync();
        }
        public async Task<List<RecommendedViewModel>> GetPopularRecommendedAsync(int start, int size, int range)
        {

            var dateRange = DateTime.Now.AddMonths(-range); //show time ticker detail within date range
            var monthRange = DateTime.Now;
            var popularCourses = await db.courses
                .Include(c => c.classDetails)
                .Where(c => c.CourseStatus != -1 
                    && c.CourseStatus != 3 
                    && c.CourseStatus != 4 
                    && c.courserating >= 0.1
                    && c.courserating <= 5             
                )          
            .Select(c => new RecommendedViewModel
            {
                CourseId = c.id,
                CourseTitle = (c.title.Length > 30) ? c.title.Substring(0, 30) + "..." : c.title,
                CourseImage = "https://quorse.s3.amazonaws.com/pub/course/course/"+c.bgLarge,
                CourseEntityName = db.entities
                                        .Where(e => e.guid == c.entityguid)
                                        .FirstOrDefault().name,
                CoursePrice =c.courseprice,
                CourseRating = c.courserating,
                CourseIsUsd= c.isusd,
                CourseClassTTEndDate = c.classDetails
                    .Where(d => d.istimeticker == true
                            && d.status == 1
                            && DateTime.Now >= d.ttstartdate
                            && d.ttenddate >= DateTime.Now
                            && d.ttenddate < dateRange)
                    .FirstOrDefault()
                    .ttenddate,
                // get a list of class details for each course that have class date later than today
                ClassSimpleModels = c.classDetails
                    .Where(d => d.status != -1 
                    && d.status != 3 
                    && d.status != 4 
                    && d.classDates
                    .Any(e => e.date > monthRange)        //slow down the loading time
                        )
                .Select(d => new ClassComplexModel
                {
                    ClassId = d.id,
                    ClassIsTimeTicker = d.istimeticker,
                    ClassIsUsd = d.isusd,
                    ClassPrice = d.price,
                    ClassPromoPrice = d.promoprice,
                    ClassTTPrice = d.ttprice

                }),
                CourseHasELearningClass = c.classDetails
                        .Where(d => d.status != -1
                            && d.status != 3
                            && d.status != 4
                            &&d.type==(int)ClassDetailType.ELEARNING&&d.classDates.Any(e=>e.date>DateTime.Now))
                        .Count() > 0,
                CourseHasLVCClass = c.classDetails
                      .Where(d => d.status != -1
                            && d.status != 3
                            && d.status != 4
                            && d.type == (int)ClassDetailType.LIVEVIRTUAL && d.classDates.Any(e => e.date > DateTime.Now))
                        .Count() > 0,
                CourseHasPublicClass = c.classDetails
                        .Where(d => d.status != -1
                            && d.status != 3
                            && d.status != 4
                            && d.type == (int)ClassDetailType.PUBLIC && d.classDates.Any(e => e.date > DateTime.Now))
                        .Count() > 0
            })
            .Where(c=>c.ClassSimpleModels.Count()>0)
            .OrderByDescending(c => c.CourseRating)
            .Skip(start)
            .Take(size).ToListAsync();

            return  popularCourses;
        }
    }
}
