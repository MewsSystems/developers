using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        const string url = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            try
            {

                //Get exchange rate data for CZK
                List<string> exchangeRatesData = this.GetExchangeRateData(url).ToList();

                //Remove first two header lines
                exchangeRatesData.RemoveRange(0, 2);

                Currency targetCurrency = new Currency("CZK");
                List<ExchangeRate> exchangeRates = new List<ExchangeRate>();

                //Parse text data to create exchange rates for currencies that are required
                foreach (string line in exchangeRatesData)
                {
                    string[] dataLine = line.Split('|');
                    Currency currentCurrency = new Currency(dataLine[3]);

                    if (currencies.Any(x => x.Code.Equals(currentCurrency.Code)))
                    {
                        //Get the rate for 1 to 1 as some values are brought back in higher multiples e.g. THB
                        decimal rate = decimal.Parse(dataLine[4]) / decimal.Parse(dataLine[2]);

                        ExchangeRate currentExchangeRate = new ExchangeRate(currentCurrency, targetCurrency, rate);
                        exchangeRates.Add(currentExchangeRate);
                    }
                }

                return exchangeRates;
            }
            catch (Exception ex)
            {
                Console.WriteLine("*** Error getting exchange rates ***");
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        private IEnumerable<string> GetExchangeRateData(string url)
        {
            List<string> exchangeRatesData = new List<string>();

            //Get current exchange rate data
            WebClient client = new WebClient();
            using (Stream stream = client.OpenRead(url))
            {
                //Split the text into seperate lines
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        exchangeRatesData.Add(reader.ReadLine());
                    }
                }
            }

            return exchangeRatesData;
        }
    }
}
