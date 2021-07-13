using System.Collections.Generic;
using System.Linq;

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
        /// 

        const string CNB_URL = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
        const char COLUMN_SEPARATOR = '|';

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            string downloadedDataString;

            using (var webClient = new System.Net.WebClient())
            {
                downloadedDataString = webClient.DownloadString(CNB_URL);
            }

            return ProcessDownloadedString(downloadedDataString, currencies);
        }

        private IEnumerable<ExchangeRate> ProcessDownloadedString(string downloadedDataString, IEnumerable<Currency> currencies)
        {
            string line;

            int readLines = 0;

            var exchangeRates = new List<ExchangeRate>();

            using (System.IO.StringReader reader = new System.IO.StringReader(downloadedDataString))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (readLines > 1)
                    {
                        var parsedLine = line.Split(COLUMN_SEPARATOR);

                        var currencyCode = parsedLine[3];

                        if (currencies.Any(x => x.ToString() == currencyCode))
                        {
                            var receivedAmount = decimal.Parse(parsedLine[2]);
                            var requiredAmount = decimal.Parse(parsedLine[4]);

                            var normalizedRate = requiredAmount / receivedAmount;

                            var exchangeRate = new ExchangeRate(new Currency(currencyCode), new Currency("CZK"), normalizedRate);

                            exchangeRates.Add(exchangeRate);
                        }
                    }

                    readLines++;
                }

                return exchangeRates;
            }
        }
    }
}
