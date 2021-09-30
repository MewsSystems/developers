using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Factories;

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
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var exchangeRates = GetExchangeRates();

            return currencies
                .AsParallel()
                .Distinct()
                .Select(currency => exchangeRates.TryGetValue(currency, out var exchangeRate) ? exchangeRate : null)
                .Where(x => x != null);
        }

        #region ExchangeProvider

        // TODO move to config
        private const string ExchangeRateUrl = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt";

        // TODO cache data instead of calling everytime
        private static IDictionary<Currency, ExchangeRate> GetExchangeRates()
        {
            return GetExchangeRateFromCzk(DateTime.Now.Date).GetAwaiter().GetResult().ToDictionary(x => x.TargetCurrency, x => x);
        }

        private static async Task<IReadOnlyCollection<ExchangeRate>> GetExchangeRateFromCzk(DateTime date)
        {
            var exchangeRates = new List<ExchangeRate>();
            using (var httpClient = new HttpClient { BaseAddress = new Uri(ExchangeRateUrl) })
            {
                using (var responseStream = await httpClient.GetStreamAsync($"?date={date:dd.MM.yyyy}"))
                {
                    using (var streamReader = new StreamReader(responseStream))
                    {
                        try
                        {
                            string line;
                            var i = 0;
                            while ((line = await streamReader.ReadLineAsync()) != null)
                            {
                                i++;

                                if (i <= 2)
                                {
                                    continue;
                                }

                                var columns = line.Split('|');
                                if (columns.Length != 5)
                                {
                                    throw new Exception(nameof(ExchangeRateProvider));
                                }

                                exchangeRates.Add(MapLineToExchangeRate(columns));
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                    }
                }
            }

            return exchangeRates;
        }

        #region Helpers

        private static ExchangeRate MapLineToExchangeRate(IReadOnlyList<string> columns)
        {
            return ExchangeRateFactory.CreateAsTargetCzk(columns[3], ParseRate(columns[4]));
        }

        private static decimal ParseRate(string rate)
        {
            if (decimal.TryParse(rate, NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("cs-CZ"), out var decimalRate))
            {
                return decimalRate;
            }

            throw new ArgumentException("Incorrect decimal format", nameof(ParseRate));
        }

        #endregion

        #endregion
    }
}