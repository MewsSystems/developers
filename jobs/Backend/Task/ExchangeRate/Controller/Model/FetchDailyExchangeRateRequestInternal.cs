using ExchangeRateUpdater.ExchangeRate.Constant;
using ExchangeRateUpdater.ExchangeRate.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExchangeRateUpdater.ExchangeRate.Controller.Model
{
    /// <summary>
    /// Represents a request to fetch daily exchange rates.
    /// </summary>
    /// /// <summary>
    /// Initializes a new instance of the <see cref="FetchDailyExchangeRateRequestInternal"/> class.
    /// </summary>
    /// <param name="baseCurrency">The base currency.</param>
    /// <param name="date">The date.</param>
    /// <param name="language">The language.</param>
    /// <param name="targetCurrencies">The target currencies.</param>
    public class FetchDailyExchangeRateRequestInternal(Currency baseCurrency, DateOnly date, Language language, List<Currency> targetCurrencies)
    {
        /// <summary>
        /// Gets or sets the base currency.
        /// </summary>
        [Required]
        public Currency BaseCurrency { get; set; } = baseCurrency;

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        [Required]
        public DateOnly Date { get; set; } = date;

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        [Required]
        public Language Language { get; set; } = language;

        /// <summary>
        /// Gets or sets the target currencies.
        /// </summary>
        [Required]
        [MinLength(1, ErrorMessage = "TargetCurrencies must contain at least one currency.")]
        public List<Currency> TargetCurrencies { get; set; } = targetCurrencies;

        /// <summary>
        /// Returns a string representation of the object.
        /// </summary>
        /// <returns>A string representation of the object.</returns>
        public override string ToString() =>
            $"BaseCurrency: {BaseCurrency}, Date: {Date}, Language: {Language}, TargetCurrencies: {(string.Join(", ", TargetCurrencies)).Trim()}";
    }
}
