using Quorse.AppApi.BL.Interfaces;
using Quorse.AppApi.DAL.Interfaces;
using Quorse.AppApi.DAL.Repositories;
using Quorse.EntityFramework.EntityFramework.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quorse.AppApi.BL
{
    public class CurrencyService:ICurrencyService
    {
        private readonly ICurrencyRepository CurrencyRepository;
        public CurrencyService(ICurrencyRepository currencyRepository)
        {
            if (currencyRepository == null)
                throw new ArgumentNullException();

            CurrencyRepository = currencyRepository;
        }
        public CurrencyService()
        {
            CurrencyRepository = new CurrencyRepository();
        }
        public async Task<currency> GetLatestCurrencyAsync()
        {
            return await CurrencyRepository.GetLatestCurrencyAsync();
        }
    }
}
