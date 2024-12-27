using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Models
{
    [Serializable]
    public class ExchangeRateApi
    {
        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("rate")]
        public decimal Rate { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }
    }
}
