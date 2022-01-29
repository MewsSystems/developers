using System.Collections.Generic;
using ExchangeRateUpdater.Dtos;

namespace ExchangeRateUpdater.Providers.ExchangeRateProvider
{
    public interface IExchangeRateProvider
    {
        IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}