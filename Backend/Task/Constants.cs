using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class Constants
    {
        public const string Url = "https://www.cnb.cz";
        public const string RateUrl = "{0}/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
        public const string RateUrlCzech = "{0}/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";
        public const string CzechCurrencyCode = "CZK";
        public const string EnUs = "en-US";
        public const string CsCz = "cs-CZ";
        public const char Pipe = '|';
    }
}
