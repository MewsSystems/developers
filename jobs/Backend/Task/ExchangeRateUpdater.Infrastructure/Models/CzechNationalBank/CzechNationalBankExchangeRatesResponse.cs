using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Infrastructure.Models.CzechNationalBank
{
    [ExcludeFromCodeCoverage]
    public record CzechNationalBankExchangeRatesResponse
    {
        [JsonPropertyName("rates")]
        public IEnumerable<CzechNationalBankExchangeRate> Rates { get; set; } = new List<CzechNationalBankExchangeRate>();
    }
}
