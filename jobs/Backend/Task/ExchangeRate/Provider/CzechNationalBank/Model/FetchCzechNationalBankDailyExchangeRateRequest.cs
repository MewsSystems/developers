using ExchangeRateUpdater.ExchangeRate.Constant;
using System;

namespace ExchangeRateUpdater.ExchangeRate.Provider.CzechNationalBank.Model
{
    /// <summary>
    /// Represents a request to fetch daily exchange rates from the Czech National Bank.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="FetchCzechNationalBankDailyExchangeRateRequest"/> class with the specified date and language.
    /// </remarks>
    /// <param name="date">The date for which to fetch the exchange rates.</param>
    /// <param name="language">The language in which to fetch the exchange rates.</param>
    internal class FetchCzechNationalBankDailyExchangeRateRequest(DateOnly date, Language language)
    {
        /// <summary>
        /// Gets or sets the date for which to fetch the exchange rates.
        /// </summary>
        public DateOnly Date { get; set; } = date;

        /// <summary>
        /// Gets or sets the language in which to fetch the exchange rates.
        /// </summary>
        public Language Language { get; set; } = language;
    }
}
