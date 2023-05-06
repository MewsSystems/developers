using ExchangeRateUpdater;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;

public class ExchangeRateCache
{
    private readonly HttpClient httpClient;
    private readonly IExchangeRateDataSourceOptions options;
    private readonly IMemoryCache cache;

    private const string DailyRatesCacheKey = "daily_rates";
    private const string MonthlyRatesCacheKey = "monthly_rates";

    public ExchangeRateCache(HttpClient httpClient, IExchangeRateDataSourceOptions options, IMemoryCache cache)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this.options = options ?? throw new ArgumentNullException(nameof(options));
        this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public async Task<IEnumerable<ExchangeRate>> GetDailyRatesAsync(string dailyRatesUrl)
    {
        var cachedRates = cache.Get<IEnumerable<ExchangeRate>>(DailyRatesCacheKey);
        if (cachedRates != null)
        {
            return cachedRates;
        }

        var rates = new List<ExchangeRate>();

        try
        {
            var response = await new HttpClient().GetAsync($"{dailyRatesUrl}?date={DateTime.Today:dd.MM.yyyy}");
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

        // TODO: If today is a public holiday, set the end of the working day to the following working day

        // Set the end of the working day to 2.30pm on the current or next working day
        var endOfWorkingDay = today.AddHours(14).AddMinutes(30);
        if (DateTimeOffset.Now >= endOfWorkingDay)
        {
            endOfWorkingDay = endOfWorkingDay.AddDays(1);
        }

        return endOfWorkingDay;
    }

    public async Task<IEnumerable<ExchangeRate>> GetMonthlyRatesAsync(string monthlyRatesUrl)
    {
        var cachedRates = cache.Get<IEnumerable<ExchangeRate>>(MonthlyRatesCacheKey);
        if (cachedRates != null)
        {
            return cachedRates;
        }

        var rates = new List<ExchangeRate>();

        try
        {
            var response = await new HttpClient().GetAsync($"{monthlyRatesUrl}?year={DateTime.Today.Year}&month={DateTime.Today.Month - 1}");
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
