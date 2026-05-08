using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater
{
    public class ExchangeRateResponse
    {
        [JsonPropertyName("rates")]
        public List<ExchangeRateDto> Rates { get; set; } = new();
    }

    public class ExchangeRateDto
    {
        /// <summary>
        /// The date on which this rate was declared by the CNB. For commonly traded currencies, rates are declared 
        /// every working day after 2:30 p.m. and remain valid for that day and any immediately following non-working 
        /// days (weekends or public holidays). For other currencies, rates are declared on the last working day of the 
        /// month and remain valid for the entire following month. This property indicates the declaration date, but 
        /// the rate itself stays valid until it is superseded by the next declared rate according to these rules.
        /// </summary>
        [JsonPropertyName("validFor")]
        public DateTime DeclarationDate { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;

        /// <summary>
        /// The quantity of the currency that the rate applies to; used to calculate the per-unit rate as Rate / Amount.
        /// </summary>
        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("currencyCode")]
        public string CurrencyCode { get; set; } = string.Empty;

        /// <summary>
        /// The exchange rate corresponding to the specified Amount; divide by Amount to obtain the per-unit rate.
        /// </summary>
        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }
    }
}