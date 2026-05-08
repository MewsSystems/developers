using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.Options
{
    public class CnbApiOptions
    {
        public const string SectionName = "CnbApi";
        public string BaseUrl { get; set; }
        public string DailyRatesPath { get; set; }
    }
}
