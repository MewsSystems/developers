using System.Collections.Generic;

namespace ExchangeRateUpdater;

public interface IExchangeRateProvider
{
    IReadOnlyCollection<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
}