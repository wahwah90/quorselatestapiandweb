using Quorse.EntityFramework.EntityFramework.Schema;
using Quorse.EntityFramework.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Quorse.AppApi.Helper
{
    public class BaseHelper
    {
        public static decimal? GetDiscountRate(decimal? normalPrice,decimal?lowestPrice)
        {
            return ((normalPrice - lowestPrice) / normalPrice) * 100;
        }
        public static decimal usdToRM(decimal? myr, bool? isUsd, decimal? usdExchangeRate)
        {
            var _myr = myr;
            if (isUsd == true)
            {
                _myr = myr * usdExchangeRate;

            }
            return (decimal)_myr;
           
        }
        //get lowest price->ttprice,promoprice,listprice
        public static decimal lowestClassPrice(ClassComplexModel cd, decimal? usdExchangeRate)
        {
            decimal lowestPrice = 0;

            //lowest to highest ->ttprice,promoprice,listprice
            if (cd.ClassIsTimeTicker == true && cd.ClassTTPrice != null && cd.ClassTTPrice > 0)
            {
                lowestPrice = (decimal)cd.ClassTTPrice;
            }
            else if (cd.ClassPromoPrice != null && cd.ClassPromoPrice > 0)
            {
                lowestPrice = (decimal)cd.ClassPromoPrice;
            }
            else
            {
                lowestPrice = (decimal)cd.ClassPrice;
            }

            //check price is in usd
            lowestPrice =  usdToRM(lowestPrice, cd.ClassIsUsd, usdExchangeRate);
            return lowestPrice;
        }
    }
}