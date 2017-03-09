using Quorse.EntityFramework.EntityFramework.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Quorse.AppApi.DAL.Repositories.Base
{
    public class BaseFuncRepository
    {
        private QuorseDbEntities db = new QuorseDbEntities();
        
        public BaseFuncRepository() {  }

        //convert to myr from usd
        protected decimal usdToRM(decimal? myr, bool? isUsd,decimal? usdExchangeRate)
        {
            var _myr = myr;
            if (isUsd==true)
            {
                _myr = myr* usdExchangeRate;
               
            }
            return (decimal)_myr;
        }
        //get lowest price->ttprice,promoprice,listprice
        protected decimal lowestClassPrice(classDetail cd,decimal? usdExchangeRate)
        {
            decimal lowestPrice = 0;

            //lowest to highest ->ttprice,promoprice,listprice
            if (cd.istimeticker==true&&cd.ttprice != null && cd.ttprice > 0)
            {
                lowestPrice = (decimal) cd.ttprice;
            }
            else if (cd.promoprice != null && cd.promoprice > 0)
            {
                lowestPrice = (decimal)cd.promoprice;
            }
            else
            {
                lowestPrice = (decimal)cd.price;
            }

            //check price is in usd
            lowestPrice = usdToRM(lowestPrice, cd.isusd,usdExchangeRate);
            return lowestPrice;
        }
    }
}
