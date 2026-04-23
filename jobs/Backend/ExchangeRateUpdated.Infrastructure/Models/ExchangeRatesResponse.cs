using Newtonsoft.Json;

namespace ExchangeRateUpdater.Domain.Models
{
    public class ExchangeRatesResponse
    {
        [JsonProperty("rates")]
        public List<ExchangeRateRow>? Rates { get; set; }
    }
}
