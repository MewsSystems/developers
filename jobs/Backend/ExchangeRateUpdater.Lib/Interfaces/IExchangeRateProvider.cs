using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Interfaces;

public interface IExchangeRateProvider
{
    public Task<IEnumerable<ExchangeRate>> GetExchangeRates();
}