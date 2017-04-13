using Newtonsoft.Json;
using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public class CurrencyResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("terms")]
        public string Terms { get; set; }

        [JsonProperty("privacy")]
        public string Privacy { get; set; }

        [JsonProperty("timestamp")]
        public int Timestamp { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("quotes")]
        public Dictionary<string, decimal> Quotes { get; set; }
    }
}

