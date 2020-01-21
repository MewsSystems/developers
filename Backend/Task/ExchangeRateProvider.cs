using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// Questions:
        /// 
        /// The better option is to read from StreamReader, parse line by line, token by token, insted of reading whole website and using Split()
        /// but this would lead to complicated code
        /// In our way the file can be too large to fit into memory. The size is: sum(sizesOfLines)
        /// The better way has memory usage: max(sizesOfLines)
        /// 
        /// There can be multiple instances of same Currency (in memory)

        /// My main problem was to find the appropriate website
        /// I am still not sure if the website is correct

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            // one line per one exchange rate
            string[] lines = DownloadExchangeRateString().Split('\n');

            // holder of ExchangeRates
            List<ExchangeRate> rates = new List<ExchangeRate>();

            // we cut first two lines
            for (int i = 2; i < lines.Count() - 1; i++) // last line is empty!!!
            {
                // we parse tokens on each line with exchange rate
                string[] line = lines[i].Split('|');

                // code of foreign currency
                string code = line[3];

                if (currencies.Any(c => c.Code == code))
                {
                    // wa calculate propper exchange rate
                    int ammounth = int.Parse(line[2]);
                    float rate = float.Parse(line[4]);

                    // we add ExchangeRate into list of exchange rates
                    rates.Add(new ExchangeRate(new Currency("CZK"), new Currency(code), (decimal)(ammounth / rate)));
                }
            }

            return rates;
        }

        private string DownloadExchangeRateString()
        {
            // we download website into the string s
            const string webAddress = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt?date=%d"; //date=%d to get current date
            WebClient client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            Stream data = client.OpenRead(webAddress);
            StreamReader reader = new StreamReader(data);
            return reader.ReadToEnd();
        }
    }
}
