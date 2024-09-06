using ExchangeRateUpdater.ExchangeRate.Constant;
using ExchangeRateUpdater.ExchangeRate.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRate.Provider
{
    /// <summary>
    /// Represents a provider for fetching exchange rate data.
    /// </summary>
    public interface IExchangeRateProvider
    {
        /// <summary>
        /// Gets the daily exchange rates for the specified target date and language.
        /// </summary>
        /// <param name="targetDate">The target date for which to fetch exchange rates.</param>
        /// <param name="language">The language in which to fetch exchange rates.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation, returning the daily exchange rates.</returns>
        Task<IEnumerable<ExchangeRateData>> GetDailyExchangeRates(DateOnly targetDate, Language language, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the supported languages for fetching exchange rates.
        /// </summary>
        /// <returns>The supported languages.</returns>
        IEnumerable<Language> GetSupportedLanguages();
    }
}
