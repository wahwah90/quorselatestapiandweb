﻿using Quorse.AppApi.BL.Interfaces;
using Quorse.AppApi.DAL.Interfaces;
using Quorse.AppApi.DAL.Repositories;
using Quorse.EntityFramework.EntityFramework.Schema;
using Quorse.EntityFramework.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quorse.AppApi.BL.Services
{
    public class CourseService:ICourseService
    {
        private readonly ICourseRepository CourseRepository;
        public CourseService(ICourseRepository courseRepository)
        {
            if (courseRepository == null)
                throw new ArgumentNullException();

            CourseRepository = courseRepository;
        }
        public CourseService()
        {
            CourseRepository = new CourseRepository();
        }
        public IQueryable<course> Courses()
        {
            return CourseRepository.GetAll();
        }
        public async Task<course> CoursesAsync(int id)
        {
            
            return await CourseRepository.GetByKeyAsync(id);
        }
        public async Task<int> UpdateCourseAsync(course course)
        {
            return await CourseRepository.SaveAsync(course);
        }
        public async Task<int> AddCourseAsync(course course)
        {
            return await CourseRepository.SaveAsync(course);
        }
        public async Task<course> DeleteCourseAsync(int key)
        {
            return await CourseRepository.DeleteAsync(key);
        }
        public IQueryable<RecommendedViewModel> GetTimeTickerRecommended(int start, int size, int range)
        {
            return CourseRepository.GetTimeTickerRecommended(start, size, range);
        }
        //public IQueryable<RecommendedViewModel> GetLatestRecommended(int start, int size, int range)
        //{
        //    return CourseRepository.GetLatestRecommended(start, size, range);
        //}
        public async Task<List<RecommendedViewModel>> GetPopularRecommendedAsync(int start, int size, int range)
        {
            return await CourseRepository.GetPopularRecommendedAsync(start, size, range);
        }
        public async Task<List<RecommendedViewModel>> GetPopularCourseAsync(int start, int size, int range)
        {
            return await CourseRepository.GetPopularCourseAsync(start, size, range);
        }
        public async Task<List<RecommendedViewModel>> GetLatestCourseAsync(int start, int size, int range)
        {
            return await CourseRepository.GetLatestCourseAsync(start, size, range);
        }

        public async Task<List<RecommendedViewModel>> GetSuggestionCourseAsync(int start, int size, int range, int? courseLevel1, int? courseLevel2, int? courseLevel3, int? courseLevel4)
        {
            return await CourseRepository.GetSuggestionCourseAsync(start, size, range, courseLevel1, courseLevel2, courseLevel3, courseLevel4);
        }
    }
}
