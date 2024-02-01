using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Data.Models
{
    public class CnbDailyRatesResponse
    {
        public CnbDailyRatesResponse()
        {
            Rates = new List<CnbRate>();
        }

        [JsonPropertyName("rates")]
        public List<CnbRate> Rates { get; set; }
    }
}
