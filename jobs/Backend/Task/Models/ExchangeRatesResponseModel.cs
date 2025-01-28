using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Models
{
    public class ExchangeRatesResponseModel
    {
        [JsonPropertyName("rates")]
        public List<ExchangeRatesModel> ExchangeRates { get; set; }
    }
}
