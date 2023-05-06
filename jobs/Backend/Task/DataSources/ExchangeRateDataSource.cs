using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.DataSources
{
    public class ExchangeRateDataSource : IExchangeRateDataSource
    {
        private readonly IExchangeRateDataSourceOptions options;
        private readonly ExchangeRateCache exchangeRateCache;

        public ExchangeRateDataSource(IExchangeRateDataSourceOptions options, HttpClient httpClient, IMemoryCache cache)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            exchangeRateCache = new ExchangeRateCache(cache);
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
                return Enumerable.Empty<ExchangeRate>();
            }


            return rates;
        }



    }
}





