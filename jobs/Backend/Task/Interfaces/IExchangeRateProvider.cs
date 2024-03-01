using System.Collections.Generic;

namespace ExchangeRateUpdater.Interfaces;

public interface IExchangeRateProvider
{
    IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
}