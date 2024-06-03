using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.ExchangeRate.Controller.Model
{
    /// <summary>
    /// Represents the response for fetching daily exchange rates.
    /// </summary>
    /// <remarks>This class defines the structure of the response returned when fetching daily exchange rates.</remarks>
    public class FetchDailyExchangeRateResponse(string sourceCurrency, DateOnly date, IEnumerable<ExchangeRateResponseItem> exchangeRates)
    {
        /// <summary>
        /// Gets or sets the base currency.
        /// </summary>
        [JsonPropertyName("base")]
        public string SourceCurrency { get; set; } = sourceCurrency;

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <remarks>This property represents the date for which the exchange rates are fetched.</remarks>
        public DateOnly Date { get; set; } = date;

        /// <summary>
        /// Gets or sets the exchange rates.
        /// </summary>
        /// <remarks>This property represents a collection of exchange rate items.</remarks>
        [JsonPropertyName("rates")]
        public IEnumerable<ExchangeRateResponseItem> ExchangeRates { get; set; } = exchangeRates;
    }
}
