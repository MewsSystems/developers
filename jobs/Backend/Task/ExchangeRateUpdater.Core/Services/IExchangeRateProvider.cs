using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.Services;

public interface IExchangeRateProvider
{
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies);
}