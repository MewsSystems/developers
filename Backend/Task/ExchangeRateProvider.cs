using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            IExchangeRatesProvider provider = new CnbExchangeRatesProvider();
            var dic = provider.GetExchangeRates();
            foreach (var currency in currencies)
            {
                if (dic.TryGetValue(currency.Code, out var rate))
                    yield return new ExchangeRate(currency, new Currency(provider.TargetCurrency),
                        Math.Round(rate, 3));
            }
            //return Enumerable.Empty<ExchangeRate>();
        }


        public interface IExchangeRatesProvider
        {
            string TargetCurrency { get; }
            Dictionary<string, decimal> GetExchangeRates();
        }


        public class CnbExchangeRatesProvider : IExchangeRatesProvider
        {
            public string TargetCurrency => "CZK";

            public Dictionary<string, decimal> GetExchangeRates()
            {
                var date = DateTime.Now.Date;
                string content;
                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    content = client.DownloadString(
                        $"https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt?date={date.ToString("dd.MM.yyyy")}");
                }

                string[] rows = content.Split('\n');
                var headerRow = rows[1].Split('|');
                int amountIndex = Array.IndexOf(headerRow, "množství");
                int codeIndex = Array.IndexOf(headerRow, "kód");
                int basicRateIndex = Array.IndexOf(headerRow, "kurz");
                Dictionary<string, decimal> rates = new Dictionary<string, decimal>();
                for (int i = 2; i < rows.Length; i++)
                {
                    if (rows[i].Length == 0)
                        continue;
                    var row = rows[i].Split('|');
                    rates.Add(row[codeIndex], decimal.Parse(row[basicRateIndex]) / int.Parse(row[amountIndex]));
                }

                //if(!rates.ContainsKey(TargetCurrency))
                //    rates.Add(TargetCurrency, 1);
                return rates;
            }
        }
    }
}
