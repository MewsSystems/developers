﻿using ExchangeRateProvider.Contract.API;
using ExchangeRateProvider.Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly ExchageRateProviderApi _api;

        public ExchangeRateProvider(string apiUrl, string apiKey)
        {
            _api = new ExchageRateProviderApi(apiUrl, apiKey);
        }


        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            try
            {
                return Task.Run(() => _api.GetExchangeRatesAsync(currencies)).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ExchangeRateProvider.GetExchangeRates: Error while getting exchange rates from api (ex. {ex.Message})");
                return null;
            }
        }
    }
}
