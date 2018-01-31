using System.Collections.Generic;

namespace ExchangeRateUpdater.Providers
{
    public interface IExchangeRateProvider
    {
        IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies); 
    }
}
