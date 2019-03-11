using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public interface IExchangeRatesFilter
    {
        IEnumerable<ExchangeRate> GetFilteredRates(IEnumerable<ExchangeRate> unfilteredRates,
            IEnumerable<Currency> currencies);
    }
}
