using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.CzechNationalBank
{
    internal class CzechNationalBankExchangeRatesResponse
    {
        [JsonPropertyName("rates")]
        public ExchangeRate[] Rates { get; set; }

        internal class ExchangeRate
        {
            [JsonPropertyName("currencyCode")]
            public string CurrencyCode { get; set; }

            [JsonPropertyName("rate")]
            public decimal Rate { get; set; }
        }
    }
}
