using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.CbnApiClient.DTO
{
    /// <summary>
    /// Represents an individual exchange rate entry as returned by the CBN API. 
    /// Each entry contains details about the currency, its exchange rate, and the date for which the rate is valid.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ExchangeRateEntry
    {
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;

        [JsonPropertyName("currencyCode")]
        public string CurrencyCode { get; set; } = string.Empty;

        [JsonPropertyName("order")]
        public int Order { get; set; }

        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }

        // Date the rate is valid for. The API returns a date string (e.g. "2026-02-15").
        [JsonPropertyName("validFor")]
        public DateTime ValidFor { get; set; }
        
    }
}
