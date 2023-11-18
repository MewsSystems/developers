using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Api.Models
{
    public class DailyExchangeRatesRequest
    {
        [JsonPropertyName("currencies")]
        public List<Currency> Currencies { get; set; }
    }
}
