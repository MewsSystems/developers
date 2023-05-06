﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater
{
    public class ExchangeRateDataSource : IExchangeRateDataSource
    {
        private readonly IExchangeRateDataSourceOptions options;
        private readonly ExchangeRateCache exchangeRateCache;

        public ExchangeRateDataSource(IExchangeRateDataSourceOptions options, HttpClient httpClient, IMemoryCache cache)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.exchangeRateCache = new ExchangeRateCache(cache);
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var rates = new List<ExchangeRate>();

            try
            {
                var dailyRates = await exchangeRateCache.GetDailyRatesAsync(options.DailyRatesUrl);
                var monthlyRates = await exchangeRateCache.GetMonthlyRatesAsync(options.MonthlyRatesUrl);

                foreach (var currency in currencies)
                {
                    var dailyRatesForCurrency = dailyRates.Where(r => r.SourceCurrency.Code == currency.Code);
                    var monthlyRatesForCurrency = monthlyRates.Where(r => r.SourceCurrency.Code == currency.Code);

                    rates.AddRange(dailyRatesForCurrency);
                    rates.AddRange(monthlyRatesForCurrency);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving the exchange rates: {ex.Message}");
            }

            return rates;
        }

    }
}





