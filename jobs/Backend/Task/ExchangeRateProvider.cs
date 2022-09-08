using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {

        private static readonly string DAILY_EXCHANGE_RATE_URL = "http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt";
        private static Dictionary<string, ExchangeRate> _exchangeRatesCache = new Dictionary<string, ExchangeRate>();


        public static void LoadFromServer()
        {
            try
            {
                _exchangeRatesCache.Clear();

                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
                {
                    using (var response = client.GetAsync(DAILY_EXCHANGE_RATE_URL, System.Net.Http.HttpCompletionOption.ResponseHeadersRead).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (var stream = response.Content.ReadAsStreamAsync().Result)
                            {
                                using (var sReader = new StreamReader(stream))
                                {
                                    while (!sReader.EndOfStream)
                                    {
                                        string rawData = sReader.ReadLine();

                                        ParseData(rawData);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                _exchangeRatesCache.Clear();
            }
        }

        private static void ParseData(string rawData)
        {
            try
            {
                string header = "země|měna|množství|kód|kurz";
                if (rawData.Equals(header))
                    return;

                var regex = new System.Text.RegularExpressions.Regex(@"^([0-9]{2})[.]([0-9]{2})[.]([0-9]{4})");
                if (regex.IsMatch(rawData))
                    return;

                ParseExchangeRate(rawData);
            }
            catch (Exception)
            {
            }
        }

        private static void ParseExchangeRate(string rawData)
        {
            try
            {
                char separator = '|';

                string[] parsedDataArray = rawData.Split(separator);
                if (parsedDataArray.Length < 0)
                    return;

                Decimal.TryParse(parsedDataArray[4], out var exchangeRate);
                //will use only currency code
                _exchangeRatesCache.Add(
                    parsedDataArray[3],
                    new ExchangeRate(new Currency("CZK"), new Currency(parsedDataArray[3]), exchangeRate));
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            return _exchangeRatesCache.Values.Where(x => x.TargetCurrency == currencies);
        }
    }
}
