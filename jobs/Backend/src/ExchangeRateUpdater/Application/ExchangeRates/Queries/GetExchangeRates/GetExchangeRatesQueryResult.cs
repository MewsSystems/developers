using ExchangeRateUpdater.Application.Models;

namespace ExchangeRateUpdater.Application.ExchangeRates.Queries.GetExchangeRates
{
    public class GetExchangeRatesQueryResult
    {
        public IEnumerable<ExchangeRate> ExchangeRates { get; set; }
    }
}
