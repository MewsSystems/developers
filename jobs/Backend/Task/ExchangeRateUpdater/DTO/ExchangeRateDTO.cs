using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.DTO
{
    internal class ExchangeRateDTO
    {
        [JsonProperty("validFor")]
        public DateTime ValidFor;

        [JsonProperty("currencyCode")]
        public string CurrencyCode;

        [JsonProperty("rate")]
        public decimal Rate;
    }
}
