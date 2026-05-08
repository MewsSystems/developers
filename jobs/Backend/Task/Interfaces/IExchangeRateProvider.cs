using System.Collections.Generic;

namespace ExchangeRateUpdater.Interfaces
{
    public interface IExchangeRateProvider
    {
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}
