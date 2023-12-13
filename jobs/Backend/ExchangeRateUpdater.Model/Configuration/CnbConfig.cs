using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Model.Configuration
{
    public class CnbConfig
    {
        public string BaseAddress { get; set; }
        public string ApiEndpoint { get; set; }
        public string TargetCurrency { get; set; }
    }
}
