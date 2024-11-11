using ExchangeRateUpdater.Models;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Mappers
{
    public static class ExchangeRateMapper
    {
        public static IEnumerable<ExchangeRate> MapApiFxRatesToExchangeRates(IEnumerable<FxRate> apiFxRates)
        {
            return apiFxRates?.Select(x => new ExchangeRate(new Currency(x.CurrencyCode), new Currency("CZK"), (decimal)x.Rate))
                ?? Enumerable.Empty<ExchangeRate>();
        }
    }
}
