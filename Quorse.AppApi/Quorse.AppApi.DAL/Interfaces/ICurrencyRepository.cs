using Quorse.AppApi.DAL.Interfaces.Base;
using Quorse.EntityFramework.EntityFramework.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quorse.AppApi.DAL.Interfaces
{
    public interface ICurrencyRepository : IBaseRepository<currency, int>
    {
        Task<currency> GetLatestCurrencyAsync();
    }
}
