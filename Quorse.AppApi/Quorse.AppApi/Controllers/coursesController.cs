using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Quorse.EntityFramework.EntityFramework.Schema;
using Quorse.AppApi.BL.Interfaces;
using Quorse.AppApi.BL.Services;
using Quorse.EntityFramework.ViewModels;
using System.Web.Http.Cors;
using Quorse.AppApi.Helper;
using static Quorse.AppApi.Helper.ClassDetailTypeConverter;

namespace Quorse.AppApi.Controllers
{
    public class coursesController : ApiController
    {
        private readonly ICourseService _CourseService;
        private readonly ICurrencyService _CurrencyService;
        private readonly IClassDetailService _ClassDetailService;
        private readonly IEntityService _EntityService;
        public coursesController(ICourseService courseService,ICurrencyService currencyService,IClassDetailService classDetailService,IEntityService entityService)
        {
            _CourseService = courseService;
            _CurrencyService = currencyService;
            _ClassDetailService = classDetailService;
            _EntityService = entityService;
        }
        public IQueryable<course> Getcourses()
        {
            var courses = _CourseService.Courses();
            return _CourseService.Courses();
        }

        [ResponseType(typeof(course))]
        public async Task<IHttpActionResult> Getcourse(int id)
        {
            course course = await _CourseService.CoursesAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putcourse(course course)
        {
            int courseId = 0;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                courseId = await _CourseService.UpdateCourseAsync(course);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (courseId <= 0)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(course))]
        public async Task<IHttpActionResult> Postcourse(course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _CourseService.AddCourseAsync(course);

            return CreatedAtRoute("DefaultApi", new { id = course.id }, course);
        }

        [ResponseType(typeof(course))]
        public async Task<IHttpActionResult> Deletecourse(int id)
        {
            course course = await _CourseService.DeleteCourseAsync(id);
            if (course == null)
            {
                return NotFound();
            }
         
            return Ok(course);
        }

        [HttpGet]
        //[Authorize(Roles = "Administrators")]
        public IQueryable<RecommendedViewModel> GetTimeTickers()
        {
            return _CourseService.GetTimeTickerRecommended(1, 5, 14);
        }

        [HttpGet]
        //get couses that have clases that organised in half year ago with highest rating
        public async Task<IHttpActionResult> GetLatestLazyLoadingAsync()
        {
            var usdExchangeRate = await _CurrencyService.GetLatestCurrencyAsync();
            var courses = await _CourseService.GetLatestCourseAsync(1, 5, 4);
            if (courses == null || usdExchangeRate == null)
            {
                return NotFound();
            }
            foreach (var course in courses)
            {
                var classSimpleModel = await _ClassDetailService.GetClassesAsync(course.CourseId);
                var entity = await _EntityService.GetEntityByGuidAsync(course.CourseEntityName);
                var ttEndTime = await _ClassDetailService.GetTTEndTime(course.CourseId, 14);
                if (entity != null)
                    course.CourseEntityName = entity.name;
                if (ttEndTime != null)
                    course.CourseClassTTEndDate = ttEndTime;
                if (classSimpleModel != null && classSimpleModel.Count() > 0)
                {
                    var lowestPriceClass = classSimpleModel
                                      .Select(c => new
                                      {
                                          LowestPrice = BaseHelper.lowestClassPrice(c, usdExchangeRate.CurrencyRate),
                                          NormalPrice = BaseHelper.usdToRM(c.ClassPrice, c.ClassIsUsd, usdExchangeRate.CurrencyRate)
                                      })
                                      .OrderBy(c => c.LowestPrice).FirstOrDefault();
                    course.ClassSimpleModel = new ClassSimpleModel();
                    course.ClassSimpleModel.CourseLowestClassPrice = lowestPriceClass.LowestPrice;
                    course.ClassSimpleModel.CourseHighestClassPrice = lowestPriceClass.NormalPrice;
                    course.ClassSimpleModel.CourseDiscountRate = BaseHelper.GetDiscountRate(lowestPriceClass.NormalPrice, lowestPriceClass.LowestPrice);
                    course.CourseHasELearningClass = await _ClassDetailService.HasTypeOfClassAsync(course.CourseId, (int)ClassDetailType.ELEARNING);
                    course.CourseHasLVCClass = await _ClassDetailService.HasTypeOfClassAsync(course.CourseId, (int)ClassDetailType.LIVEVIRTUAL);
                    course.CourseHasPublicClass = await _ClassDetailService.HasTypeOfClassAsync(course.CourseId, (int)ClassDetailType.PUBLIC);
                }
            }

            return Ok(courses);
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetPopular()
        {
            var usdExchangeRate = await _CurrencyService.GetLatestCurrencyAsync();
            var courses = await _CourseService.GetPopularRecommendedAsync(1, 5, 4);
            if (courses == null || usdExchangeRate == null)
            {
                return NotFound();
            }
            foreach (var ccourse in courses)
            {
                ccourse.ClassSimpleModel = new ClassSimpleModel();
                if (ccourse.CourseIsUsd==true)
                {
                    ccourse.CoursePrice =ccourse.CoursePrice * usdExchangeRate.CurrencyRate;
                }
                if (ccourse.ClassSimpleModels != null&&ccourse.ClassSimpleModels.Count()>0)
                {
                    var lowestPriceClass = ccourse.ClassSimpleModels
                   .Select(c => new
                   {
                       LowestPrice = BaseHelper.lowestClassPrice(c, usdExchangeRate.CurrencyRate),
                       NormalPrice = BaseHelper.usdToRM(c.ClassPrice, c.ClassIsUsd, usdExchangeRate.CurrencyRate)
                   })
                   .OrderBy(c => c.LowestPrice).FirstOrDefault();

                    ccourse.ClassSimpleModel.CourseLowestClassPrice = lowestPriceClass.LowestPrice;
                    ccourse.ClassSimpleModel.CourseHighestClassPrice = lowestPriceClass.NormalPrice;
                    ccourse.ClassSimpleModel.CourseDiscountRate = ((lowestPriceClass.NormalPrice - lowestPriceClass.LowestPrice) / lowestPriceClass.NormalPrice) * 100;
                }
               
                ccourse.ClassSimpleModels = null;
            }
            return Ok(courses);
           
        }
        [HttpGet]
        //get couses that have clases that organised in half year ago with highest rating
        public async Task<IHttpActionResult> GetPopularLazyLoadingAsync()
        {
            var usdExchangeRate = await _CurrencyService.GetLatestCurrencyAsync();
            var courses = await _CourseService.GetPopularCourseAsync(1, 5, 14);
            if (courses == null || usdExchangeRate == null)
            {
                return NotFound();
            }
            foreach (var course in courses)
            {
                var classSimpleModel = await _ClassDetailService.GetClassesAsync(course.CourseId);
                var entity = await _EntityService.GetEntityByGuidAsync(course.CourseEntityName);
                var ttEndTime = await _ClassDetailService.GetTTEndTime(course.CourseId, 14);
                if(entity!=null)
                    course.CourseEntityName = entity.name;
                if (ttEndTime != null)
                    course.CourseClassTTEndDate = ttEndTime;
                if (classSimpleModel != null && classSimpleModel.Count() > 0)
                {
                    var lowestPriceClass = classSimpleModel
                                      .Select(c => new
                                      {
                                          LowestPrice = BaseHelper.lowestClassPrice(c, usdExchangeRate.CurrencyRate),
                                          NormalPrice = BaseHelper.usdToRM(c.ClassPrice, c.ClassIsUsd, usdExchangeRate.CurrencyRate)
                                      })
                                      .OrderBy(c => c.LowestPrice).FirstOrDefault();
                    course.ClassSimpleModel = new ClassSimpleModel();
                    course.ClassSimpleModel.CourseLowestClassPrice = lowestPriceClass.LowestPrice;
                    course.ClassSimpleModel.CourseHighestClassPrice = lowestPriceClass.NormalPrice;
                    course.ClassSimpleModel.CourseDiscountRate = BaseHelper.GetDiscountRate(lowestPriceClass.NormalPrice, lowestPriceClass.LowestPrice);
                    course.CourseHasELearningClass =  await _ClassDetailService.HasTypeOfClassAsync(course.CourseId,(int)ClassDetailType.ELEARNING);
                    course.CourseHasLVCClass = await _ClassDetailService.HasTypeOfClassAsync(course.CourseId, (int)ClassDetailType.LIVEVIRTUAL);
                    course.CourseHasPublicClass = await _ClassDetailService.HasTypeOfClassAsync(course.CourseId, (int)ClassDetailType.PUBLIC);
                }           
            }

            return Ok(courses);
        }

        [HttpGet]
        //get couses that have clases that organised in half year ago with highest rating
        public async Task<IHttpActionResult> GetSuggestionLazyLoadingAsync()
        {
            var usdExchangeRate = await _CurrencyService.GetLatestCurrencyAsync();
            var courses = await _CourseService.GetSuggestionCourseAsync(1, 5,14,3,163,322,385);
            if (courses == null || usdExchangeRate == null)
            {
                return NotFound();
            }
            foreach (var course in courses)
            {
                var classSimpleModel = await _ClassDetailService.GetClassesAsync(course.CourseId);
                var entity = await _EntityService.GetEntityByGuidAsync(course.CourseEntityName);
                var ttEndTime = await _ClassDetailService.GetTTEndTime(course.CourseId, 14);
                if (entity != null)
                    course.CourseEntityName = entity.name;
                if (ttEndTime != null)
                    course.CourseClassTTEndDate = ttEndTime;
                if (classSimpleModel != null && classSimpleModel.Count() > 0)
                {
                    var lowestPriceClass = classSimpleModel
                                      .Select(c => new
                                      {
                                          LowestPrice = BaseHelper.lowestClassPrice(c, usdExchangeRate.CurrencyRate),
                                          NormalPrice = BaseHelper.usdToRM(c.ClassPrice, c.ClassIsUsd, usdExchangeRate.CurrencyRate)
                                      })
                                      .OrderBy(c => c.LowestPrice).FirstOrDefault();
                    course.ClassSimpleModel = new ClassSimpleModel();
                    course.ClassSimpleModel.CourseLowestClassPrice = lowestPriceClass.LowestPrice;
                    course.ClassSimpleModel.CourseHighestClassPrice = lowestPriceClass.NormalPrice;
                    course.ClassSimpleModel.CourseDiscountRate = BaseHelper.GetDiscountRate(lowestPriceClass.NormalPrice, lowestPriceClass.LowestPrice);
                    course.CourseHasELearningClass = await _ClassDetailService.HasTypeOfClassAsync(course.CourseId, (int)ClassDetailType.ELEARNING);
                    course.CourseHasLVCClass = await _ClassDetailService.HasTypeOfClassAsync(course.CourseId, (int)ClassDetailType.LIVEVIRTUAL);
                    course.CourseHasPublicClass = await _ClassDetailService.HasTypeOfClassAsync(course.CourseId, (int)ClassDetailType.PUBLIC);
                }
            }

            return Ok(courses);
        }
    }
}