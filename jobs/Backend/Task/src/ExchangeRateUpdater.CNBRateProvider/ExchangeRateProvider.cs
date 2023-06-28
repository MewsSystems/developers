using ExchangeRateUpdater.Domain.Models;

namespace ExchangeRateUpdater.CNBRateProvider;

internal class ExchangeRateProvider : IExchangeRateProvider
{
    public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        // return new[] { new ExchangeRate(new Currency("USD"), new Currency("CZK"), 2) };
        return Enumerable.Empty<ExchangeRate>();
    }
}

