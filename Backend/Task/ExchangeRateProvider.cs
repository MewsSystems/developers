using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {

        private const string sourceMain = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";
        private const string sourceOther = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-ostatnich-men/kurzy-ostatnich-men/kurzy.txt";

        private const char lineSeparator = '\n';
        private const char itemSeparator = '|';
        private const string destinationCurrency = "CZK";
        private const int amountPosition = 2;
        private const int sourcePosition = 3;
        private const int ratePosition = 4;
        private const int startingLine = 3;

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            HashSet<string> requestedCurrencies = new HashSet<string>();
            foreach (var currency in currencies)
            {
                requestedCurrencies.Add(currency.Code);
            }

            NumberFormatInfo formatInfo = new NumberFormatInfo();
            formatInfo.NumberDecimalSeparator = ",";

            using (HttpClient client = new HttpClient())
            {
                foreach (var source in new string[] { sourceMain, sourceOther })
                {
                    var result = client.GetStringAsync(source).Result;
                    int lineNumber = 1;
                    foreach (var line in result.Split(lineSeparator))
                    {
                        //line format  Austrálie|dolar|1|AUD|15,389
                        //skip two lines
                        if (lineNumber >= startingLine && line.Trim() != string.Empty)
                        {
                            var items = line.Split(itemSeparator);
                            int amount = int.Parse(items[amountPosition]);
                            string sourceCurrency = items[sourcePosition];
                            string destination = destinationCurrency;
                            if (requestedCurrencies.Contains(sourceCurrency) && requestedCurrencies.Contains(destination))
                            {
                                decimal rate = decimal.Parse(items[ratePosition], formatInfo);
                                yield return new ExchangeRate(new Currency(sourceCurrency), new Currency(destination), rate / amount);
                            }
                        }
                        lineNumber++;
                    }
                }
            }
        }
    }
}
