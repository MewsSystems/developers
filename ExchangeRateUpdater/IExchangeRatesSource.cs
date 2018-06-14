using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public interface IExchangeRatesSource
    {
        /// <summary>
        /// Returns all exchange rates provided by source
        /// </summary>
        /// <returns></returns>
        IEnumerable<ExchangeRate> LoadExchangeRates();
    }
}