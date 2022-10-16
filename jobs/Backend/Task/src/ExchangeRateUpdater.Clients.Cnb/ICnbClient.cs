using ExchangeRateUpdater.Clients.Cnb.Responses;

namespace ExchangeRateUpdater.Clients.Cnb;

/// <summary>
/// Cnb client.
/// </summary>
public interface ICnbClient
{
    /// <summary>
    /// Gets exchange rates from Cnb.
    /// </summary>
    /// <returns>An awaitable task of <see cref="ExchangeRatesResponse"/>.</returns>
    Task<ExchangeRatesResponse> GetExchangeRatesAsync();
}