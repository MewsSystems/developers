using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Domain.Model.Cnb.Rs
{
    public class CnbExchangeRatesRsModel
    {
        [JsonPropertyName("rates")]
        public CnbExchangeRatesRsModelRate[] Rates { get; set; }

        public class CnbExchangeRatesRsModelRate
        {
            [JsonPropertyName("amount")]
            public int Amount { get; set; }
            [JsonPropertyName("currencyCode")]
            public string CurrencyCode { get; set; }
            [JsonPropertyName("rate")]
            public decimal Rate { get; set; }
        }

    }
}
