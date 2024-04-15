using ExchangeRateUpdater.Domain.ValueObjects;

namespace ExchangeRateUpdater.Domain.Interfaces
{
    /// <summary>
    /// Represents an interface for fetching exchange rates.
    /// </summary>
    public interface IExchangeRateProvider
    {
        /// <summary>
        /// Asynchronously retrieves exchange rates for the specified date and currencies.
        /// </summary>
        /// <param name="date">The date for which exchange rates are requested. If null, the latest available rates are fetched.</param>
        /// <param name="currencies">The currencies for which exchange rates are requested. If empty, all available rates are fetched.</param>
        /// <returns>A task representing the asynchronous operation, returning a collection of exchange rates.</returns>
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(DateOnly? date, IEnumerable<Currency> currencies);
    }
}
