using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Singleton;

namespace ExchangeRateUpdater.Chain_of_Responsibility
{
    internal class Redis : Handler
    {
        private DB _rates;

        public Redis() => _rates = DB.GetInstance();

        public override ExchangeRate GetExchangeRate(Currency currency)
        {
            if (_rates.TryGetValue(currency.Code, out Rate rate))
            {
                return new ExchangeRate(currency, new Currency("CZK"), rate.rate);
            }
            else if(!_rates.IsEmpty())
            {
                return null;
            }

            return next.GetExchangeRate(currency);
        }
    }
}
