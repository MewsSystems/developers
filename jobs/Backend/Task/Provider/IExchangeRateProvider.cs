using System.Collections.Generic;
using ExchangeRateUpdater.DTO;

namespace ExchangeRateUpdater.Provider;

public interface IExchangeRateProvider
{
    IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
}