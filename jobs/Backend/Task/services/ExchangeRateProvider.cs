using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.model;

namespace ExchangeRateUpdater.services;

public class ExchangeRateProvider : IExchangeRateProvider
{
    public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        return Enumerable.Empty<ExchangeRate>();
    }
}