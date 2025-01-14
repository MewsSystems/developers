using ExchangeRateUpdater.DTOs;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services;

public interface IExchangeRateService
{
    /// <summary>
    /// Retrieves the exchange rates asynchronously.
    /// If the exchange rates are available in the cache, it returns the cached rates.
    /// Otherwise, calls an external api, caches the result, and returns it.
    /// </summary>
    /// <returns>A task result containing the exchange rates.</returns>
    /// <exception cref="Exception">Thrown when the HTTP request to get exchange rates fails.</exception>
    Task<ExchangeRatesDTO> GetExchangeRatesAsync();
}
