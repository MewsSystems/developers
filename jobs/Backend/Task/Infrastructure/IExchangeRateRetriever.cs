using ExchangeRateUpdater.Common;
using ExchangeRateUpdater.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure;

public interface IExchangeRateRetriever
{
    Task<Result<ExchangeRate[]>> GetExchangeRatesAsync();
}
