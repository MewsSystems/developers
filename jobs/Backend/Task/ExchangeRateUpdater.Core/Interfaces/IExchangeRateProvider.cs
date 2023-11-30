using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.Interfaces;

public interface IExchangeRateProvider
{
    public Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies);
}