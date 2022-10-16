using ExchangeRateUpdater.Domain.Models;

namespace ExchangeRateUpdater.Domain.Providers;

public interface IExchangeRateProviderClient
{
    /// <summary>
    /// Gets exchange rates from defined client.
    /// </summary>
    /// <returns>An awaitable task of <see cref="ExchangeRate"/>.</returns>
    Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync();
}