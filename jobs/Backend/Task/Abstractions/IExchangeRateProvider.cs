using ExchangeRateUpdater.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Abstractions;

public interface IExchangeRateProvider
{
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies);
    IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
}