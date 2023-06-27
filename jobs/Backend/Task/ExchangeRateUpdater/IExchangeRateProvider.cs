using System.Collections.Generic;

namespace ExchangeRateUpdater;

public interface IExchangeRateProvider
{
    IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
}