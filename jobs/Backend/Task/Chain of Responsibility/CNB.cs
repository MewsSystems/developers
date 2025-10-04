using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateUpdater.CNB;
using ExchangeRateUpdater.Decorator;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Singleton;

namespace ExchangeRateUpdater.Chain_of_Responsibility
{
    internal class CNB : Handler
    {
        private LoadRates _load;

        public CNB() => _load = new APICall(new DataClean(new LoadData()));

        public override ExchangeRate GetExchangeRate(Currency currency)
        {
            bool result = _load.Load("01.01.2003").Result;

            if (DB.GetInstance().TryGetValue(currency.Code, out Rate rate))
            {
                return new ExchangeRate(currency, new Currency("CZK"), rate.rate);
            }

            return null;
        }
    }
}
