using System;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Models
{
    public class CurrencyIsoData
    {
        [JsonPropertyName("entity")]
        public string Entity { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("alphabeticcode")]
        public string AlphabeticCode { get; set; }

        [JsonPropertyName("numericcode")]
        public string NumericCode { get; set; }

        [JsonPropertyName("minorunit")]
        public string MinorUnit { get; set; }

        [JsonPropertyName("withdrawaldate")]
        public string WithdrawalDate { get; set; }

        public bool IsActive => string.IsNullOrEmpty(WithdrawalDate);
    }
} 