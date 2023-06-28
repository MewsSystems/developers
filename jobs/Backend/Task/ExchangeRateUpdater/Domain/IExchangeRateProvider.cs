using System.Collections.Generic;

namespace ExchangeRateUpdater.Domain;

public interface IExchangeRateProvider
{
    IReadOnlyCollection<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
}