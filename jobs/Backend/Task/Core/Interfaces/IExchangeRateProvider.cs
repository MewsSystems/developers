using ExchangeRateUpdater.Core.Entities;

namespace ExchangeRateUpdater.Core.Interfaces;

public interface IExchangeRateProvider
{
    Task<IList<ExchangeRate>> GetExchangeRatesAsync();
}