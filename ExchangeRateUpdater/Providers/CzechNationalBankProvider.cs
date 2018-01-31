using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ExchangeRateUpdater.Providers
{
    public class CzechNationalBankProvider:IExchangeRateProvider
    {
        private const string SourceUrl = "http://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt";
        private const char Separator = '|';
        private const char NewLine = '\n';
        private static readonly Currency BaseCurrency = new Currency("CZK"); 

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if(currencies==null) throw new Exception("Currencies should be set");
            string sourceData = string.Empty;
            using (var webClient = new WebClient() {Encoding= System.Text.Encoding.UTF8 })
            {
                sourceData = webClient.DownloadString(SourceUrl) ?? throw new Exception("Source of data is empty");
            }
            var lines = sourceData.Split(new char[] { NewLine }, StringSplitOptions.RemoveEmptyEntries);
            Currency targetCurrency; decimal rate; 
            for (int i = 2; i < lines.Count(); i++)
            {
                try
                {
                    var parts = lines[i].Split(Separator);
                    if (parts.Count() != 5) throw new Exception();
                    targetCurrency = new Currency(parts[3]?.ToUpper());
                    if (!currencies.Select(n => n.Code).Contains(targetCurrency?.Code?.ToUpper())) continue;
                    rate = decimal.Parse(parts[4]) / decimal.Parse(parts[2]);
                }
                catch { throw new Exception("Parsing error"); }
                yield return (new ExchangeRate(BaseCurrency, targetCurrency, rate));
            } 
        }
    }
}
