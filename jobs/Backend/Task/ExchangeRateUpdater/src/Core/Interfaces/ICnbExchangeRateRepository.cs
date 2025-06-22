using ExchangeRateUpdater.Core.Models;
namespace ExchangeRateUpdater.Core.Interfaces
{
    /// <summary>
    /// Interface for a repository that retrieves exchange rates from the Czech National Bank (CNB) API.
    /// </summary>
    public interface ICnbExchangeRateRepository
    {
        /// <summary>
        /// Asynchronously retrieves the latest exchange rates.
        /// </summary>
        /// <returns>A collection of <see cref="ExchangeRate"/> objects representing the latest exchange rates.</returns>
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync();

        /// <summary>
        /// Asynchronously retrieves specific exchange rates for the given currencies.
        /// </summary>
        /// <param name="currencies"></param>
        /// <returns></returns>
        Task<IEnumerable<ExchangeRate>> GetSpecificExchangeRatesAsync(IEnumerable<Currency> currencies);
    }
}