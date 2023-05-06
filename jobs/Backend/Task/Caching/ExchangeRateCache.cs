using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Security.Authentication;
using ExchangeRateUpdater.Models;

public class ExchangeRateCache
{
    private readonly IMemoryCache cache;
    private readonly HttpClient httpClient;

    private const string DailyRatesCacheKey = "daily_rates";
    private const string MonthlyRatesCacheKey = "monthly_rates";

    public ExchangeRateCache(IMemoryCache cache, HttpClient httpClient = null)
    {
        this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
        this.httpClient = httpClient ?? new HttpClient();
    }

    public async Task<IEnumerable<ExchangeRate>> GetDailyRatesAsync(string dailyRatesUrl)
    {
        var cachedRates = cache.Get<IEnumerable<ExchangeRate>>(DailyRatesCacheKey);
        if (cachedRates != null)
        {
            return cachedRates;
        }

        var content = await GetRatesAsync(dailyRatesUrl, DateTime.Today);
        var rates = ExchangeRateParser.Parse(content, Currency.CZK);

        var endOfWorkingDay = GetEndOfWorkingDay();
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(endOfWorkingDay.Subtract(DateTimeOffset.Now.LocalDateTime));

        cache.Set(DailyRatesCacheKey, rates, cacheEntryOptions);

        return rates;
    }

    private async Task<string> GetRatesAsync(string ratesUrl, DateTime date)
    {
        using var handler = new HttpClientHandler();
        // Accept all certificates
        handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

        using var client = new HttpClient(handler);
        // Set TLS version and SSL/TLS protocol version
        handler.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls;

        var response = await client.GetAsync($"{ratesUrl}?date={date:dd.MM.yyyy}");

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error fetching rates from {ratesUrl}: {(int)response.StatusCode} {response.ReasonPhrase}");
        }

        return await response.Content.ReadAsStringAsync();
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

        var content = await GetRatesAsync(monthlyRatesUrl, new DateTime(DateTime.Today.Year, DateTime.Today.Month - 1, 1));
        var rates = ExchangeRateParser.Parse(content, Currency.CZK);

        var lastDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1);
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(lastDayOfMonth.AddDays(1).Subtract(DateTimeOffset.Now.LocalDateTime));
        cache.Set(MonthlyRatesCacheKey, rates, cacheEntryOptions);

        return rates;
    }
}