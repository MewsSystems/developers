using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Cnb
{
    internal class CnbExchangeRates
    {
        [JsonPropertyName("rates")]
        [JsonInclude]
        public IEnumerable<CnbExchangeRate> Rates { get; private set; } = null!; // required and json don't work well
    }

    internal sealed class CnbExchangeRate
    {
        [JsonPropertyName("currencyCode")] public string CurrencyCode { get; init; } = null!; // required and json don't work well

        [JsonPropertyName("amount")] public int Amount { get; init; }

        [JsonPropertyName("rate")] public decimal Rate { get; init; }

        public decimal RealRate => Rate / Amount;
    }
}
