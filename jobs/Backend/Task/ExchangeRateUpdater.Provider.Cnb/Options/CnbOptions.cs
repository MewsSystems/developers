using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Provider.Cnb.Options
{
    public class CnbOptions
    {
        public string BaseUrl { get; set; }
        public CnbPaths Paths { get; set; }
        public string SourceCurrencyCode { get; set; }
    }

    public class CnbPaths
    {
        public string DailyExrates { get; set; }
    }
}
