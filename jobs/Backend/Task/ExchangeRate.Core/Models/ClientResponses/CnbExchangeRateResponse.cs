using System.Text.Json.Serialization;

namespace ExchangeRate.Core.Models.ClientResponses
{
    public class CnbExchangeRateResponse
    {
        [JsonPropertyName("rates")]
        public IEnumerable<CnbExchangeRate> Rates { get; set; } = Enumerable.Empty<CnbExchangeRate>();
    }
}
