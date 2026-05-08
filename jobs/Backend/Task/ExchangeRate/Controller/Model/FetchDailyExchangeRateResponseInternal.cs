using ExchangeRateUpdater.ExchangeRate.Model;
using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater.ExchangeRate.Controller.Model
{
    /// <summary>
    /// Represents the internal response for fetching daily exchange rates.
    /// </summary>
    /// <remarks>This class defines the structure of the internal response returned when fetching daily exchange rates.</remarks>
    /// <remarks>
    /// Initializes a new instance of the <see cref="FetchDailyExchangeRateResponseInternal"/> class.
    /// </remarks>
    /// <param name="sourceCurrency">The source currency.</param>
    /// <param name="date">The date.</param>
    /// <param name="exchangeRates">The exchange rates.</param>
    public class FetchDailyExchangeRateResponseInternal(Currency sourceCurrency, DateOnly date, IEnumerable<ExchangeRateData> exchangeRates)
    {

        /// <summary>
        /// Gets or sets the source currency.
        /// </summary>
        public Currency SourceCurrency { get; set; } = sourceCurrency ?? throw new ArgumentNullException(nameof(sourceCurrency));

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateOnly Date { get; set; } = date;

        /// <summary>
        /// Gets or sets the exchange rates.
        /// </summary>
        public IEnumerable<ExchangeRateData> ExchangeRates { get; set; } = exchangeRates ?? throw new ArgumentNullException(nameof(exchangeRates));
    }
}
