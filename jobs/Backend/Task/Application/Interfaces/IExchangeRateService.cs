using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.Application.Interfaces
{
    /// <summary>
    /// Application service interface for retrieving exchange rates.
    /// Orchestrates multiple exchange rate providers.
    /// </summary>
    public interface IExchangeRateService
    {
        /// <summary>
        /// Retrieves exchange rates for the requested currencies asynchronously.
        /// Only returns rates explicitly provided by the underlying providers.
        /// </summary>
        /// <param name="requestedCurrencies">Currencies to fetch rates for.</param>
        /// <returns>Collection of exchange rates.</returns>
        Task<IEnumerable<ExchangeRate>> GetRatesAsync(IEnumerable<Currency> requestedCurrencies);
    }
}
