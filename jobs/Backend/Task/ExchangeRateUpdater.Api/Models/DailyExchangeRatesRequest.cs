using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Api.Models
{
    public class DailyExchangeRatesRequest
    {
        [JsonPropertyName("currency_codes")]
        public List<string> CurrencyCodes { get; set; }
    }
}
