using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private string[] rawRates;
        private string today;
        private const string URI_RATES = "http://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt?date={0}";
        private const string NUM_SEP = ".";
        private const string DATE_FORMAT = "DD.MM.YYYY";
        private const char EOL_SIMBOL = '\n';
        private const char DELIM = '|';
        

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            
            var numFormat = new NumberFormatInfo();
            numFormat.CurrencyDecimalSeparator = NUM_SEP;
            if (rawRates == null & today != DateTime.Now.ToString(DATE_FORMAT))
            {
                this.today = DateTime.Now.ToString(DATE_FORMAT);
                using (var webClient = new WebClient())
                {
                    this.rawRates = webClient.DownloadString(string.Format(URI_RATES, today)).Split(EOL_SIMBOL);
                }
            }
            for (int i = 2; i < rawRates.Length; i++)
            {
                if (rawRates[i].Length < 1) break;  
                var row = rawRates[i].Split(DELIM);                                 
                var sCur = new Currency(row[3]);
                
                // Could not find any source for other currencies than CZK
                //I suppose CNB not providing other

                var tCur = new Currency("CZK");
                if (currencies.Contains<Currency>(sCur) & currencies.Contains<Currency>(tCur))
                {
                   yield return (new ExchangeRate(sCur, tCur, Convert.ToDecimal(row[4], numFormat)));
                }
            }
            
        }
    }
}
