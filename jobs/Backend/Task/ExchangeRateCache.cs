/*
   System:         N/A
   Component:      ExchangeRateCache
   Filename:       ExchangeRateCache.cs
   Created:        31/07/2023
   Original Author:Tom Doran
   Purpose:        Cache Exchange Rates (auto-refresh at 2:30pm)
 */

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateUpdater.Helpers;
using ExchangeRateUpdater.Model;

namespace ExchangeRateUpdater
{
    public class ExchangeRateCache
    {
        private static Dictionary<string, ExchangeRate> _exchangeRates;
        private static HttpClient _client;
        private static readonly object _cacheLock = new();
        // TODO: make uris configurable
        private static readonly string _cnbUri = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date=";
        private static readonly string _localUri = "https://localhost:9000";

        /// <summary>
        /// Method for cleaing the cache.
        /// </summary>
        public static void Clear()
        {
            lock (_cacheLock)
            {
                _exchangeRates = new Dictionary<string, ExchangeRate>();
            }
        }

        /// <summary>
        /// Method for returning CZK exchange rate for the supplied currency.
        /// </summary>
        /// <param name="currencyCode"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<ExchangeRate> GetExchangeRateAsync(string currencyCode)
        {
            // if cache has not been built, build it 
            if (_exchangeRates == null)
            {
                await RefreshCache();
            }
            // return FX rate for the currency code if one is available & throw an exception if not
            if (_exchangeRates.TryGetValue(currencyCode, out var exchangeRate))
            {
                return exchangeRate;
            }
            else
            {
                throw new Exception($"No exchange rate was available for {currencyCode}.");
            }
        }

        /// <summary>
        /// Method for retrieving the cache (for testing purposes)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static Dictionary<string, ExchangeRate> GetCache()
        {
            if (_exchangeRates == null)
            {
                _exchangeRates = new Dictionary<string, ExchangeRate>();
            }
            return _exchangeRates;
        }

        /// <summary>
        /// Refreshes the cache from source data.
        /// </summary>
        /// <returns></returns>
        public static async Task RefreshCache()
        {
            // build HTTP client for calling the CNB API
            _client = new HttpClient
            {
                BaseAddress = new Uri(_localUri)
            };
            Console.WriteLine($"Building Cache of exchange rate values at {DateTime.Now}.");
            var response = await _client.GetAsync(BuildUri(DateTime.Today));

            // if request was successful
            if (response.IsSuccessStatusCode)
            {
                Clear();
                // lock in case there are other threads trying to access the cache during building
                lock (_cacheLock)
                {
                    // read response as a string & parse into ExchangeRate objects
                    var contentString = response.Content.ReadAsStringAsync().Result;
                    var exchangeRates = ExchangeRateParser.ParseResponseToKorunaRates(contentString);
                    foreach (var exchangeRate in exchangeRates)
                    {
                        _exchangeRates[exchangeRate.TargetCurrency.Code] = exchangeRate;
                    }
                }
                Console.WriteLine($"Cache populated with {_exchangeRates.Count} exchange rates.");
            }
            else
            {
                // TODO: proper logging?
                Console.WriteLine($"Exception: API call returned an erroneous status code.");
            }
        }

        private static Uri BuildUri(DateTime date)
        {
            var dateParam = date.ToString("dd.MM.yy");
            var stringBuilder = new StringBuilder(_cnbUri).Append(dateParam);
            return new Uri(stringBuilder.ToString());
        }
    }
}
