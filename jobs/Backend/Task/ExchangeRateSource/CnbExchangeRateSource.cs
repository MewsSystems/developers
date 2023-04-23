using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.ExchangeRateSource
{
    internal class CnbExchangeRateSource : IExchangeRateSource
    {
        private const string CzechCurrencyCode = "CNZ";
        private const string CzechNationalBankBaseUrl = "https://api.cnb.cz/cnbapi/";
        private const string CzechNationalBankExchangeRateEndpoint = "exrates/daily";
        public CnbExchangeRateSource()
        {
            CurrencyCode = new Currency(CzechCurrencyCode);
            BaseRestUrl = CzechNationalBankBaseUrl;
            EndPoint = CzechNationalBankExchangeRateEndpoint;
        }
        public Currency CurrencyCode { get; }
        public string BaseRestUrl { get; }
        public string EndPoint { get; set; }
    }
}
