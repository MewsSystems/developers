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
        ///
        
        ///this will return actual exchande rates.
        private const string URL = "http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt";
        private readonly NumberStyles style = NumberStyles.AllowDecimalPoint;
        private readonly CultureInfo culture = CultureInfo.CreateSpecificCulture("cs-CZ");

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();

            StreamReader responseReader = getAcualExchangeRateReader();

            Currency CZK = new Currency("CZK");

            while (!responseReader.EndOfStream)
            {
                string line = responseReader.ReadLine();

                var splitted = line.Split('|');

                if (splitted.Count() != 5)
                    continue;

                if (!currencies.Any(c => c.Code.Equals(splitted[3])))
                    continue;

                if (!decimal.TryParse(splitted[4], style, culture, out decimal amount) ||
                    !decimal.TryParse(splitted[2], style, culture, out decimal rate))
                    continue;

                Currency currentCurrency = new Currency(splitted[3]);
                exchangeRates.Add(new ExchangeRate(currentCurrency, CZK, decimal.Divide(amount, rate)));
            }

            return exchangeRates;
        }

        private StreamReader getAcualExchangeRateReader()
        {
            StreamReader responseReader = new StreamReader(new MemoryStream());

            try
            {
                WebRequest request = WebRequest.Create(URL);
                WebResponse response = request.GetResponse();
                responseReader = new StreamReader(response.GetResponseStream());
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Exception: " + e.Message);
                Console.Error.WriteLine("Stack trace:");
                Console.Error.WriteLine(e.StackTrace);
            }

            return responseReader;

        }

    }
}
