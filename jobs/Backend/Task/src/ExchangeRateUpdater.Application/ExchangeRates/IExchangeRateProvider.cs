using ExchangeRateUpdater.Application.Common;
using ExchangeRateUpdater.Application.Models;

namespace ExchangeRateUpdater.Application.ExchangeRates;

public interface IExchangeRateProvider
{
    public Task<Result<IEnumerable<ExchangeRate>>> GetExchangeRates(IEnumerable<Currency> currencies);
}
