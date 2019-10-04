using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;

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
            var textData = DataDownloader();
            var exchangeRates = DataParser(textData, currencies);

            return exchangeRates;
        }

        public List<string> DataDownloader()
        {
            List<string> listOfTextLines = new List<string>();

            try
            {
                WebClient client = new WebClient();
                StreamReader reader = new StreamReader(
                    // Central bank exchange rate fixing - link downloads current data in txt format in english culture settings
                    client.OpenRead("https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt")
                );

                string text;
                while ((text = reader.ReadLine()) != null)
                {
                    listOfTextLines.Add(text);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            // table header remove
            listOfTextLines.RemoveRange(0, 2);

            return listOfTextLines;
        }

        public IEnumerable<ExchangeRate> DataParser(List<string> listOfTextLines, IEnumerable<Currency> currencies)
        {
            if (listOfTextLines == null)
            {
                throw new ArgumentNullException("No data provided.");
            }

            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();
            CultureInfo culture = new CultureInfo("en-US");

            foreach (var line in listOfTextLines)
            {
                // skip if no string is passed
                if (string.IsNullOrEmpty(line))
                    continue;

                // example line - Austrálie|dolar|1|AUD|23,282
                var fields = line.Split('|');

                var sourceCurrency = new Currency(fields[3]);

                // skip if the currency is not contained in the list of our currencies
                if (currencies.FirstOrDefault(x => x.Code == sourceCurrency.Code) == null)
                    continue;

                // current format of the source provides rate into CZK only
                var targetCurrency = new Currency("CZK");
                var parsedExchangeRate = decimal.Parse(fields[4], culture);
                var parsedMultiplier = decimal.Parse(fields[2], culture);

                var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, parsedExchangeRate / parsedMultiplier);
                exchangeRates.Add(exchangeRate);
            }

            return exchangeRates;
        }
    }
}
