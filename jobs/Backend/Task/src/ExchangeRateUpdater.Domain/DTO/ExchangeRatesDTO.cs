using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Domain.DTO
{
    public class ExchangeRatesDTO
    {
        [JsonPropertyName("rates")]
        public IEnumerable<ExchangeRateDTO> Rates { get; set; } = new List<ExchangeRateDTO>();
    }
}
