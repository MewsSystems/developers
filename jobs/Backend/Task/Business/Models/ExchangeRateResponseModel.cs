using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExchangeRateUpdater.Business.Models
{
    public class ExchangeRateResponseModel
    {
        [JsonPropertyName("rates")]
        public List<ExchangeRateModel> ExchangeRates{ get; set; }
    }
}
