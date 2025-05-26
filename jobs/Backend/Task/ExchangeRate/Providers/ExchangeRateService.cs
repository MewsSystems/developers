using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ExchangeRateUpdater.HttpClients;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Infrastructure.Cache;
using Microsoft.Extensions.DependencyInjection;
using ExchangeRateUpdater.ExchangeRate.Dtos;
using ExchangeRateUpdater.Common.Extensions;
using ExchangeRateUpdater.Common.Constants;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.ExchangeRate.Providers
{
    public class ExchangeRateService(
        ICzechApiClient czechApiClient,
        ILogger<ExchangeRateService> logger,
        [FromKeyedServices(AppConstants.DailyRatesKeyedService)] ICnbRatesCache dailyRatesCache,
        [FromKeyedServices(AppConstants.MonthlyRatesKeyedService)] ICnbRatesCache monthlyRatesCache) : IExchangeRateService
    {
        private readonly ICzechApiClient _czechApiClient = czechApiClient;
        private readonly ILogger<ExchangeRateService> _logger = logger;
        private readonly ICnbRatesCache _dailyRatesCache = dailyRatesCache;
        private readonly ICnbRatesCache _monthlyRatesCache = monthlyRatesCache;
        private const string DailyRatesJsonUrl = "/exrates/daily?lang=EN";
        private const string MonthlyRatesJsonUrl = "/fxrates/daily-month?lang=EN&yearMonth={0}";
        private static readonly Currency CZK = new("CZK");

        public async Task<List<Models.ExchangeRate>> GetExchangeRateAsync(IEnumerable<Currency> currencies)
        {
            try
            {
                var dailyRates = await GetDailyRatesAsync();
                var monthlyRates = await GetMonthlyRatesAsync();

                var currencyCodes = new HashSet<string>(currencies.Select(c => c.Code), StringComparer.OrdinalIgnoreCase);
                var result = new List<Models.ExchangeRate>();

                foreach (var code in currencyCodes)
                {
                    if (dailyRates.TryGetValue(code, out var value))
                    {
                        result.Add(new Models.ExchangeRate(new Currency(code), CZK, value));
                    }
                    else if (monthlyRates.TryGetValue(code, out var mValue))
                    {
                        result.Add(new Models.ExchangeRate(new Currency(code), CZK, mValue));
                    }
                    else
                    {
                        _logger.LogWarning("Currency {CurrencyCode} not found in CNB daily or monthly data.", code);
                    }
                }
                _logger.LogInformation("Returning {Count} exchange rates.", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get exchange rates for requested currencies.");
                throw;
            }
        }

        private async Task<Dictionary<string, decimal>> GetDailyRatesAsync()
        {
            try
            {
                return await _dailyRatesCache.GetOrCreateAsync(async () =>
                {
                    _logger.LogInformation("Fetching daily exchange rates from CNB JSON API...");
                    var rawJson = await _czechApiClient.GetAsync(DailyRatesJsonUrl);
                    return ParseRates(rawJson);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get daily exchange rates from cache or API.");
                throw;
            }
        }

        private async Task<Dictionary<string, decimal>> GetMonthlyRatesAsync()
        {
            var yearMonth = DateTimeExtensions.GetPreviousYearMonthUtc();
            try
            {
                return await _monthlyRatesCache.GetOrCreateAsync(async () =>
                {
                    _logger.LogInformation("Fetching monthly exchange rates from CNB JSON API for {YearMonth}...", yearMonth);
                    var rawJson = await _czechApiClient.GetAsync(string.Format(MonthlyRatesJsonUrl, yearMonth));
                    return ParseRates(rawJson);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get monthly exchange rates from cache or API for {YearMonth}.", yearMonth);
                throw;
            }
        }

        private Dictionary<string, decimal> ParseRates(string rawJson)
        {
            var ratesResponse = JsonSerializer.Deserialize<CnbRatesResponse>(rawJson);
            var dict = ratesResponse?.Rates?
                .ToDictionary(
                    r => r.CurrencyCode,
                    r => r.Rate / r.Amount,
                    StringComparer.OrdinalIgnoreCase)
                ?? new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);

            dict["CZK"] = 1m;
            return dict;
        }
    }
}
