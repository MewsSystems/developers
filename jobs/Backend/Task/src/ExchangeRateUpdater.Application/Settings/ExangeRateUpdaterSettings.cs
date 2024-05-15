using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Application.Settings
{
    public class ExangeRateUpdaterSettings
    {
        public ExchangeRateProviderSettings ExchangeRateProviderSettings { get; set; }
        public string[] CurrenciesToExchange { get; set; }

        public string ExchangeTo { get; set; }
    }
}
