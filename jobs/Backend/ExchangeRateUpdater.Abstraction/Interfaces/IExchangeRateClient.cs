using ExchangeRateUpdater.Abstraction.Model;

namespace ExchangeRateUpdater.Abstraction.Interfaces
{
    /// <summary>
    /// Provides an abstraction for retrieving exchange rates for specified currencies from an external source.
    /// </summary>
    public interface IExchangeRateClient
    {
        /// <summary>
        /// Retrieves the current exchange rates for the specified currencies from an external source.
        /// </summary>
        /// <param name="validDate">An optional date parameter to specify the date for which the exchange rates should be valid. If not provided, the method will
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of
        /// ExchangeRate objects.</returns>
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(DateTime? validDate = null);
    }
}
