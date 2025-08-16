using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Data.Models
{
    public class CnbRate
    {
        //{
        //  "rates": [
        //      {
        //        "validFor": "2019-05-17",
        //        "order": 94,
        //        "country": "Austrálie",
        //        "currency": "dolar",
        //        "amount": 1,
        //        "currencyCode": "AUD",
        //        "rate": 15.858
        //      }
        //  ]
        //}
        [JsonPropertyName("validFor")]
        public DateTime ValidFor { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }


        [JsonPropertyName("country")]
        public string Country { get; set; }


        [JsonPropertyName("currency")]
        public string Currency { get; set; }


        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }
    }
}
