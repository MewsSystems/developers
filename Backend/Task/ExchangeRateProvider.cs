using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();
            try
            {
                Console.Write("Downloading exchange rates from CNB…");
                exchangeRates = GetRatesAsync(currencies).Result;
                Console.WriteLine("OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed!");
                Console.WriteLine(ex);
            }
            return exchangeRates;
        }

        public async Task<List<ExchangeRate>> GetRatesAsync(IEnumerable<Currency> currencies)
        {
            // Download content from CNB
            using (var wc = new WebClient())
            {
                wc.Encoding = System.Text.Encoding.UTF8;
                var url = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt";
                var dataString = await wc.DownloadStringTaskAsync(url);
                return ParseStringToList(dataString, currencies);
            }
        }

        public List<ExchangeRate> ParseStringToList(string ratesInSting, IEnumerable<Currency> currencies)
        {
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();
            Currency targetCurrency = new Currency("CZK");
            string aLine, aParagraph = null;
            StringReader strReader = new StringReader(ratesInSting);
            while (true)
            {
                aLine = strReader.ReadLine();
                if (aLine != null)
                {
                    foreach (var currency in currencies)
                    {
                        if (aLine.Contains(currency.Code))
                        {
                            string[] pole = aLine.Split('|');
                            Currency sourceCurrency = new Currency(pole[3]);
                            exchangeRates.Add(new ExchangeRate(sourceCurrency, targetCurrency, Convert.ToDecimal(pole[4])));
                        }
                    }
                }
                else
                {
                    aParagraph = aParagraph + "\n";
                    break;
                }
            }
            return exchangeRates;
        }

    }

}
