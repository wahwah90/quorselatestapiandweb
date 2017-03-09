using Quorse.AppApi.DAL.Interfaces;
using Quorse.AppApi.DAL.Repositories.Base;
using Quorse.EntityFramework.EntityFramework.Schema;
using Quorse.EntityFramework.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quorse.AppApi.DAL.Repositories
{
    public class ClassDetailRepository : BaseFuncRepository, IClassDetailRepository
    {
        private QuorseDbEntities db = new QuorseDbEntities();

        public Task<classDetail> DeleteAsync(int key)
        {
            throw new NotImplementedException();
        }

        public IQueryable<classDetail> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<classDetail> GetByKeyAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveAsync(classDetail entry)
        {
            throw new NotImplementedException();
        }
        public async Task<List<ClassComplexModel>> GetClassesAsync(int courseId)
        {
            return await db.classDetails.
                Where(c => c.courseid == courseId
                    &&c.status!=-1
                    &&c.status!=3
                    &&c.status!=4
                    &&c.classDates.Any(d=>d.date>DateTime.Now))
                    .Select(c=>new ClassComplexModel {
                        ClassId = c.id,
                        ClassIsTimeTicker = c.istimeticker,
                        ClassIsUsd = c.isusd,
                        ClassPrice = c.price,
                        ClassPromoPrice = c.promoprice,
                        ClassTTPrice = c.ttprice
                    })
                    .ToListAsync();
        }

        //check contains type of class
        public async Task<bool> HasTypeOfClassAsync(int courseid,int type)
        {
            return await Task.Run(()=>db.classDetails
                        .Where(d => d.courseid==courseid
                                && d.status != -1
                                && d.status != 3
                                && d.status != 4
                                && d.type == type 
                                && d.classDates
                                    .Any(e => e.date > DateTime.Now))
                                        .Count() > 0);
           
        }

        public async Task<DateTime?> GetTTEndTime(int courseid,int range)
        {
            var dateRange = DateTime.Now.AddDays(range); //show time ticker detail within date range
            return await Task.Run(()=> db.classDetails
                    .Where(d => d.courseid==courseid
                            && d.istimeticker == true
                            && d.status == 1
                            && DateTime.Now >= d.ttstartdate
                            && d.ttenddate >= DateTime.Now
                            && d.ttenddate < dateRange)
                    .FirstOrDefault()?
                    .ttenddate);
        }
    }
}