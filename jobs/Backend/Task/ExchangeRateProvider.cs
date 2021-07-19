using System.Collections.Generic;
using System.Diagnostics;
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

        const int COUNTRY_NAME_INDEX = 0;
        const int CURRENCY_NAME_INDEX = 1;
        const int RECEIVED_AMOUNT_INDEX = 2;
        const int COUNTRY_CODE_INDEX = 3;
        const int REQUIRED_AMOUNT_INDEX = 4;

        const string COUNTRY_NAME_STRING = "Country";
        const string CURRENCY_NAME_STRING = "Currency";
        const string RECEIVED_AMOUNT_STRING = "Amount";
        const string COUNTRY_CODE_STRING = "Code";
        const string REQUIRED_AMOUNT_STRING = "Rate";

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var downloadedDataString = DownloadDataFromWeb(CNB_URL);
            
            if (downloadedDataString == "")
            {
                Debug.WriteLine("Downloading exchange rates from Central National Bank didn't succeed");

                return null;
            }

            var exchangeRates = ProcessDownloadedString(downloadedDataString, currencies);

            if (exchangeRates == null)
            {
                Debug.WriteLine("Parsing exchange rates from Central National Bank didn't succeed");

                return null;
            }

            return exchangeRates;
        }

        private string DownloadDataFromWeb(string webEndpointUrl)
        {
            var downloadedDataString = "";

            using (var webClient = new System.Net.WebClient())
            {
                downloadedDataString = webClient.DownloadString(webEndpointUrl);
            }

            return downloadedDataString;
        }

        private IEnumerable<ExchangeRate> ProcessDownloadedString(string downloadedDataString, IEnumerable<Currency> currencies)
        {
            string line;

            var readLines = 0;

            var exchangeRates = new List<ExchangeRate>();

            using (System.IO.StringReader reader = new System.IO.StringReader(downloadedDataString))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (readLines > 1)
                    {
                        var exchangeRate = ParseLineToExchangeRate(line, currencies);

                        if (exchangeRate != null)
                        {
                            exchangeRates.Add(exchangeRate);
                        }
                    }
                    else if (readLines == 1)
                    {
                        var dataInGoodState = VerifyDataTemplate(line);

                        if (!dataInGoodState)
                        {
                            Debug.WriteLine("Data from Central National Bank don't match implemented column specification");

                            return null;
                        }
                    }

                    readLines++;
                }

                return exchangeRates;
            }
        }

        private ExchangeRate ParseLineToExchangeRate(string line, IEnumerable<Currency> currencies)
        {
            var parsedLine = line.Split(COLUMN_SEPARATOR);

            var currencyCode = parsedLine[COUNTRY_CODE_INDEX];

            if (currencies.Any(x => x.ToString() == currencyCode))
            {
                var receivedAmount = decimal.Parse(parsedLine[RECEIVED_AMOUNT_INDEX]);
                var requiredAmount = decimal.Parse(parsedLine[REQUIRED_AMOUNT_INDEX]);

                var normalizedRate = requiredAmount / receivedAmount;

                var exchangeRate = new ExchangeRate(new Currency(currencyCode), new Currency("CZK"), normalizedRate);

                return exchangeRate;
            }

            return null;
        }

        private bool VerifyDataTemplate(string line)
        {
            var parsedLine = line.Split(COLUMN_SEPARATOR);

            if (parsedLine[COUNTRY_NAME_INDEX] != COUNTRY_NAME_STRING ||
                parsedLine[CURRENCY_NAME_INDEX] != CURRENCY_NAME_STRING ||
                parsedLine[RECEIVED_AMOUNT_INDEX] != RECEIVED_AMOUNT_STRING ||
                parsedLine[COUNTRY_CODE_INDEX] != COUNTRY_CODE_STRING ||
                parsedLine[REQUIRED_AMOUNT_INDEX] != REQUIRED_AMOUNT_STRING
                )
            {
                return false;
            }

            return true;
        }
    }
}
