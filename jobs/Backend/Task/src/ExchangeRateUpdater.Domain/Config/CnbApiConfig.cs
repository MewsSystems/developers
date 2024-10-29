using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Domain.Config
{
    public class CnbApiConfig
    {
        public string ExchangeRateApiUrl { get; set; }
        public string LocalCurrencyIsoCode { get; set; }
        public string PreferredLanguage { get; set; }
    }
}
