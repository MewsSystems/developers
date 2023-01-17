using Newtonsoft.Json;

namespace ExchangeRateUpdater
{
    public class CzechNationalBankExchangeRate
    {
        [JsonProperty("zeme")]
        public string Country { get; set; }

        [JsonProperty("mena")]
        public string Currency { get; set; }

        [JsonProperty("mnozstvi")]
        public int Amount { get; set; }

        [JsonProperty("kod")]
        public string Code { get; set; }

        [JsonProperty("kurz")]
        public decimal Rate { get; set; }
    }
}
