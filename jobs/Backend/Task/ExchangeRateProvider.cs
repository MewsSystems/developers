using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Properties;
using System.Globalization;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private const string _defaultTargetCurrency = "CZK";
        private readonly CultureInfo cultureCZ = new CultureInfo("cs-CZ");
        private const char delimiter = '|';

        /// <summary>Gets the exchange rates (rates from the latest working day after 14:30).</summary>
        /// <param name="currencies">Requested currencies</param>
        /// <returns>Exchange rates of requested curencies</returns>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            return GetLatestRates(currencies).Result;
        }


        /// <summary>Gets the latest rates.</summary>
        /// <param name="currencies">Requested currencies</param>
        /// <returns>
        ///   Exchange rates for the requested currencies.
        /// </returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        private async Task<IEnumerable<ExchangeRate>> GetLatestRates(IEnumerable<Currency> currencies)
        {
            if (!Uri.IsWellFormedUriString(ConfigurationManager.AppSettings.Get("CNBExchangeRateSrcLink"), UriKind.Absolute))
                throw new InvalidOperationException(Resources.ErrorInvalidUrl);

            var exchangeRateList = new List<ExchangeRate>();
            using (var client = new HttpClient())
            {
                Stream data = await client.GetStreamAsync(ConfigurationManager.AppSettings.Get("CNBExchangeRateSrcLink"));

                using StreamReader sr = new StreamReader(data);
                sr.ReadLine();
                sr.ReadLine();
                string line;
                string[] parts;
                while ((line = sr.ReadLine()) != null)
                {
                    parts = line.Split(delimiter);
                    if (currencies.Any((a) => a.Code == parts[3]))
                    {
                        var elem = new ExchangeRate(parts[3], _defaultTargetCurrency, Convert.ToDecimal(parts[4], cultureCZ) / Convert.ToDecimal(parts[2], cultureCZ));
                        exchangeRateList.Add(elem);
                    }
                }
            }
            return exchangeRateList;
        }
    }
}
