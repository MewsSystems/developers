using System.Collections.Generic;
using System.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
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
        /// 

        static HttpClient client = new HttpClient();

        public async Task<String> getRawRates()
        {
            var response = await client.GetAsync("https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt");

            var stream = await response.Content.ReadAsStreamAsync();
            Console.WriteLine(response.Content);
            StreamReader readStream = new StreamReader(stream , Encoding.UTF8);
            var output = readStream.ReadToEnd();

            return output;
        }

        public IEnumerable<ExchangeRate> ParseRawRates(String rawData)
        {
            char[] delimiterChars = {'|'};

            List<ExchangeRate> rates = new List<ExchangeRate>();

            using (StringReader reader = new StringReader(rawData))
            {
                string line = string.Empty;
                do
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        string[] words = line.Split(delimiterChars);
                        if (words.Length == 5 && (string)words.GetValue(0) != "Country")
                        {
                            int divisor = int.Parse((string)words.GetValue(2));
                            var code = (string)words.GetValue(3);
                            decimal rate = decimal.Parse((string)words.GetValue(4));

                            rates.Add(new ExchangeRate(new Currency(code), new Currency("CZK"), rate / divisor));
                        }

                    }

                } while (line != null);
            }

            return rates;
        }

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var rawData = getRawRates().GetAwaiter().GetResult();

            return ParseRawRates(rawData); 
        }
    }
}
