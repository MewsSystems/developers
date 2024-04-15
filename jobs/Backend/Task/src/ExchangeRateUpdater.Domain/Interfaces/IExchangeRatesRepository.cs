using ExchangeRateUpdater.Domain.ValueObjects;

namespace ExchangeRateUpdater.Domain.Interfaces
{
    /// <summary>
    /// Represents an interface for accessing exchange rates.
    /// </summary>
    public interface IExchangeRatesRepository
    {
        /// <summary>
        /// Asynchronously retrieves exchange rates for the specified date.
        /// </summary>
        /// <param name="date">The date for which exchange rates are requested. If null, the latest available rates are fetched.</param>
        /// <returns>A task representing the asynchronous operation, returning a collection of exchange rates.</returns>
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(DateOnly? date);
    }
}
