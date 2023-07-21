using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Models.JsonModels
{
    public class Rate
    {
        public string ValidFor { get; set; }
        public int Order { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public int Amount { get; set; }
        public string CurrencyCode { get; set; }

        [JsonPropertyName("Rate")]
        public decimal ExchangeRateValue { get; set; }
    }
}
