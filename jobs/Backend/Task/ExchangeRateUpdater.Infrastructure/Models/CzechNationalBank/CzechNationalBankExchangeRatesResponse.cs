using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Infrastructure.Models.CzechNationalBank
{
    public record CzechNationalBankExchangeRatesResponse
    {
        [JsonPropertyName("rates")]
        public IEnumerable<CzechNationalBankExchangeRate> Rates { get; set; } = new List<CzechNationalBankExchangeRate>();
    }
}
