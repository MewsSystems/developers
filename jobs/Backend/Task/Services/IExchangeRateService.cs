using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.Common;
using ExchangeRateUpdater.Entities;

namespace ExchangeRateUpdater.Services;

public interface IExchangeRateService
{
    Task<Result<ExchangeRate[]>> GetExchangeRatesAsync(IEnumerable<Currency> currencies);
}