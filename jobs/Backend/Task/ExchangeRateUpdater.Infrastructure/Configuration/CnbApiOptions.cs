using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.Configuration
{
    public class CnbApiOptions
    {
        public string BaseUrl { get; set; }
        public CnbApiMethods Methods { get; set; }
    }
}
