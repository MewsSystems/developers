using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// ExchangeRatesProvider contract
    /// </summary>
    public interface IExchangeRateProvider
    {
        IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}