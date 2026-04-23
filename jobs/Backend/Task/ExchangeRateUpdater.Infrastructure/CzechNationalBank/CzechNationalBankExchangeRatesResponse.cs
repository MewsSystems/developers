using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Infrastructure.CzechNationalBank
{
    internal class CzechNationalBankExchangeRatesResponse
    {
        [JsonPropertyName("rates")]
        public ExchangeRate[] Rates { get; init; } = [];

        internal class ExchangeRate
        {
            [JsonPropertyName("amount")]
            public int Amount { get; init; }

            [JsonPropertyName("currencyCode")]
            public string CurrencyCode { get; init; } = default!;

            [JsonPropertyName("rate")]
            public decimal Rate { get; init; }
        }
    }
}
