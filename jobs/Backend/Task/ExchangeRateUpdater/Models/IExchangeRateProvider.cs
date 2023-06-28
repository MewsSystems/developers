using System.Collections.Generic;

namespace ExchangeRateUpdater.Models;

public interface IExchangeRateProvider
{
    IReadOnlyCollection<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
}