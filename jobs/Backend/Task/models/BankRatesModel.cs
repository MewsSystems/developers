using Newtonsoft.Json;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Models
{
    public class BankRatesModel
    {

        [JsonProperty(PropertyName = "kurzy")]
        public Root Root { get; set; }
    }

    public class Root
    {

        [JsonProperty(PropertyName = "tabulka")]
        public RateList RateList { get; set; }

        [JsonProperty(PropertyName = "@datum")]
        public string Date { get; set; }

    }

    public class RateList
    {

        [JsonProperty(PropertyName = "radek")]
        public List<Rate> List { get; set; }
    }
    public class Rate
    {

        [JsonProperty(PropertyName = "@kod")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "@mena")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "@mnozstvi")]
        public int Value { get; set; }

        [JsonProperty(PropertyName = "@kurz")]
        public decimal RateValue { get; set; }

    }
}