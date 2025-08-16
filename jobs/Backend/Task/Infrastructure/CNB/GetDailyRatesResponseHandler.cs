using ExchangeRateUpdater.Infrastructure.CNB.Models;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Infrastructure.CNB
{
    internal class GetDailyRatesResponseHandler
    {
        public IEnumerable<ExchangeRate> ToExchangeRates(ExRateDailyResponse response)
        {
            return response.Rates.Select(r => new ExchangeRate(new Currency(r.CurrencyCode), Currency.CZK, r.ValidFor, r.Amount, r.Rate));
        }
    }
}
