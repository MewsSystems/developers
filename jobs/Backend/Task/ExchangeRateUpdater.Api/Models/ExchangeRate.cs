using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Api.Models
{
    public class ExchangeRate
    {
        public ExchangeRate()
        {
        }

        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }

        [JsonPropertyName("source_currency")]
        public Currency? SourceCurrency { get; set; }

        [JsonPropertyName("target_currency")]
        public Currency? TargetCurrency { get; set; }

        [JsonPropertyName("value")]
        public decimal Value { get; set; }

        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }
    }
}
