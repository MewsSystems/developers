using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Data
{
    public class ExchangeRatesResponse
    {
        [JsonPropertyName("rates")]
        public ExchangeRateData[] Rates { get; set; }
    }
}
