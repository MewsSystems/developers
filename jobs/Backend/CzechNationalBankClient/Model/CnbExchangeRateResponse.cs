using System.Text.Json.Serialization;

namespace CzechNationalBankClient.Model
{
    public class CnbExchangeRateResponse
    {
        [JsonPropertyName("rates")]
        public IEnumerable<CnbExchangeRate> ExchangeRates { get; set; }
    }
}
