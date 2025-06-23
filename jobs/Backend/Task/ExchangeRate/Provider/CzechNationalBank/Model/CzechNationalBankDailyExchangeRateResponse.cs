using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.ExchangeRate.Provider.CzechNationalBank.Model
{
    /// <summary>
    /// Represents the response containing daily exchange rates from the Czech National Bank.
    /// </summary>
    internal class CzechNationalBankDailyExchangeRateResponse
    {
        /// <summary>
        /// Gets or sets the exchange rates.
        /// </summary>
        [JsonPropertyName("rates")]
        public IEnumerable<CzechNationalBankExchangeRate> ExchangeRates { get; set; }
    }

    /// <summary>
    /// Represents a daily exchange rate from the Czech National Bank.
    /// </summary>
    internal class CzechNationalBankExchangeRate
    {
        /// <summary>
        /// Gets or sets the date for which the exchange rate is valid.
        /// </summary>
        [JsonPropertyName("validFor")]
        public DateOnly ValidFor { get; set; }

        /// <summary>
        /// Gets or sets the order of the exchange rate.
        /// </summary>
        [JsonPropertyName("order")]
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the country associated with the exchange rate.
        /// </summary>
        [JsonPropertyName("country")]
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the name of the currency.
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the amount of the currency.
        /// </summary>
        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        /// <summary>
        /// Gets or sets the currency code.
        /// </summary>
        [JsonPropertyName("currencyCode")]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets the exchange rate.
        /// </summary>
        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }
    }
}
