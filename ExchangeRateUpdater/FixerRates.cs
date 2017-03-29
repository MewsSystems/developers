using System.Collections.Generic;
using Newtonsoft.Json;

namespace ExchangeRateUpdater
{
    public class FixerRates :IRate
    {
        [JsonProperty("base")]
        public string CurrencyCode;
        [JsonProperty("rates")]
        public Dictionary<string, decimal> Rates;

        public string GetCurrencyCode() => CurrencyCode;

        public Dictionary<string, decimal> GetRates() => Rates;
    }
}
