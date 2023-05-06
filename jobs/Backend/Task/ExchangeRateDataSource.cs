using System;
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
        private readonly HttpClient httpClient;
        private readonly IExchangeRateDataSourceOptions options;
        private readonly IMemoryCache cache;

        private const string DailyRatesCacheKey = "daily_rates";
        private const string MonthlyRatesCacheKey = "monthly_rates";

        public ExchangeRateDataSource(IExchangeRateDataSourceOptions options, HttpClient httpClient, IMemoryCache cache)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var rates = new List<ExchangeRate>();

            try
            {
                var dailyRates = await GetDailyRatesAsync();
                var monthlyRates = await GetMonthlyRatesAsync();

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

        private async Task<IEnumerable<ExchangeRate>> GetDailyRatesAsync()
        {
            var cachedRates = cache.Get<IEnumerable<ExchangeRate>>(DailyRatesCacheKey);
            if (cachedRates != null)
            {
                return cachedRates;
            }

            var rates = new List<ExchangeRate>();

            try
            {
                var response = await httpClient.GetAsync($"{options.DailyRatesUrl}?date={DateTime.Today:dd.MM.yyyy}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                rates.AddRange(ParseExchangeRates(content, Currency.CZK));

                var endOfWorkingDay = GetEndOfWorkingDay();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(endOfWorkingDay.Subtract(DateTimeOffset.Now.LocalDateTime));


                cache.Set(DailyRatesCacheKey, rates, cacheEntryOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving the daily exchange rates: {ex.Message}");
            }

            return rates;
        }

        private DateTimeOffset GetEndOfWorkingDay()
        {
            var today = DateTimeOffset.Now.Date;

            // If today is a weekend, set the end of the working day to the following Monday
            if (today.DayOfWeek == DayOfWeek.Saturday || today.DayOfWeek == DayOfWeek.Sunday)
            {
                today = today.AddDays(8 - (int)today.DayOfWeek);
            }

            // If today is a public holiday, set the end of the working day to the following working day
            var publicHolidays = new List<DateTimeOffset>
            {
                new DateTimeOffset(2023, 5, 8, 0, 0, 0, TimeSpan.Zero), // example public holiday
                // add more public holidays here...
            };
                    while (publicHolidays.Contains(today))
                    {
                        today = today.AddDays(1);
                    }

            // Set the end of the working day to 2.30pm on the current or next working day
            var endOfWorkingDay = today.AddHours(14).AddMinutes(30);
            if (DateTimeOffset.Now >= endOfWorkingDay)
            {
                endOfWorkingDay = endOfWorkingDay.AddDays(1);
            }

            return endOfWorkingDay;
        }


        private async Task<IEnumerable<ExchangeRate>> GetMonthlyRatesAsync()
        {
            var cachedRates = cache.Get<IEnumerable<ExchangeRate>>(MonthlyRatesCacheKey);
            if (cachedRates != null)
            {
                return cachedRates;
            }

            var rates = new List<ExchangeRate>();

            try
            {
                var response = await httpClient.GetAsync($"{options.MonthlyRatesUrl}?year={DateTime.Today.Year}&month={DateTime.Today.Month - 1}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                rates.AddRange(ParseExchangeRates(content, Currency.CZK));

                var lastDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1);
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(lastDayOfMonth.AddDays(1).Subtract(DateTimeOffset.Now.LocalDateTime));
                cache.Set(MonthlyRatesCacheKey, rates, cacheEntryOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving the monthly exchange rates: {ex.Message}");
            }

            return rates;
        }

        private List<ExchangeRate> ParseExchangeRates(string content, Currency targetCurrency)
        {
            var exchangeRates = new List<ExchangeRate>();

            var lines = content.Split('\n');

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue;

                var parts = line.Split('|');

                if (parts.Length >= 5)
                {
                    var sourceAmount = parts[2];
                    var sourceCode = parts[3];
                    var sourceRate = parts[4];

                    decimal sourceAmountDecimal, sourceRateDecimal;
                    if (decimal.TryParse(sourceAmount, out sourceAmountDecimal) && decimal.TryParse(sourceRate, out sourceRateDecimal))
                    {
                        var calculatedRate = sourceRateDecimal / sourceAmountDecimal;
                        var exchangeRate = new ExchangeRate(new Currency(sourceCode), new Currency(targetCurrency.Code), calculatedRate);
                        exchangeRates.Add(exchangeRate);
                    }
                }
            }

            return exchangeRates;
        }
    }
}





