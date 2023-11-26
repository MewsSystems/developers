using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.Interfaces;

public interface IExchangeRateHttpClient
{
    public Task<IEnumerable<ExchangeRate>> GetExchangeRates();
}