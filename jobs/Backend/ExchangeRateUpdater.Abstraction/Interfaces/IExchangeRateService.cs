using ExchangeRateUpdater.Abstraction.Model;

namespace ExchangeRateUpdater.Abstraction.Interfaces
{
    /// <summary>
    /// Provides methods to retrieve exchange rates for specified currency codes.
    /// </summary>
    public interface IExchangeRateService
    {
        /// <summary>
        /// Retrieves a collection of exchange rates for the specified currency codes.
        /// </summary>
        /// <remarks>If any of the provided currency codes are invalid, they will be ignored in the
        /// result. The method may throw exceptions if the underlying service is unavailable or if there are issues with
        /// the provided input.</remarks>
        /// <param name="currencyCodes">An enumerable collection of currency codes for which exchange rates are to be retrieved. Each code must be a
        /// valid ISO 4217 currency code.</param>
        /// <returns>A task that represents the asynchronous operation, containing an enumerable collection of ExchangeRate
        /// objects corresponding to the provided currency codes.</returns>
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesFromStringList(IEnumerable<string> currencyCodes);
    }
}
