using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Models
{
    [Serializable]
    public class ExchangeRatesList
    {
        [JsonProperty("rates")]
        public IEnumerable<ExchangeRateApi> Rates { get; set; }
    }
}
