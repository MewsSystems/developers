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
        private static string dataSourceUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
        private static Currency czechCrown = new Currency("CZK");

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            List<ExchangeRate> rates = new List<ExchangeRate>();

            WebRequest request = WebRequest.Create(dataSourceUrl);
            using (var reader = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                // Skip the first line containing a date
                reader.ReadLine();

                string[] header = reader.ReadLine().ToLower().Split('|');
                int amountIndex = Array.IndexOf(header, "amount");
                int codeIndex = Array.IndexOf(header, "code");
                int rateIndex = Array.IndexOf(header, "rate");

                if (amountIndex == -1 || codeIndex == -1 || rateIndex == -1)
                {
                    throw new FormatException("File header is not in expected format");
                }

                while (!reader.EndOfStream)
                {
                    string[] row = reader.ReadLine().Split('|');
                    Currency currency = currencies.FirstOrDefault(c => c.Code.Equals(row[codeIndex]));
                    if (currency != null)
                    {
                        decimal.TryParse(row[rateIndex], NumberStyles.Number, CultureInfo.InvariantCulture, out decimal rate);
                        rates.Add(new ExchangeRate(currency, czechCrown, rate / int.Parse(row[amountIndex])));
                    }
                }
            }

            return rates;
        }
    }
}
