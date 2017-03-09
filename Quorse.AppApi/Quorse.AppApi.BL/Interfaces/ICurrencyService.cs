using Quorse.AppApi.BL.Interfaces.Base;
using Quorse.EntityFramework.EntityFramework.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quorse.AppApi.BL.Interfaces
{
    public interface ICurrencyService:IBaseService
    {
        Task<currency> GetLatestCurrencyAsync();
    }
}
