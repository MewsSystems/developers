using ExchangeRateUpdater.CNB;
using ExchangeRateUpdater.Decorator;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Singleton;

namespace ExchangeRateUpdater.Chain_of_Responsibility
{
    public class CNB : Handler
    {
        private LoadRates _load;

        public CNB() => _load = new APICall(new DataClean(new LoadData()));

        public CNB(LoadRates loadRates) => _load = loadRates;

        public override ExchangeRate GetExchangeRate(Currency currency)
        {
            bool result = _load.Load("").Result;

            if (DB.GetInstance().TryGetValue(currency.Code, out Rate rate))
            {
                return new ExchangeRate(currency, new Currency("CZK"), rate.rate);
            }

            return null;
        }
    }
}
