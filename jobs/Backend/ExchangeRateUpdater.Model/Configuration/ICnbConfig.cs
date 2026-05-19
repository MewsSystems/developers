using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Model.Configuration
{
    public interface ICnbConfig
    {
        public string BaseAddress { get; set; }
        public string ApiEndpoint { get; set; }
        public string TargetCurrency { get; set; }
    }
}
