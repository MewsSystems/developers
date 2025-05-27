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
        ICzechApiClient _czechApiClient,
        ILogger<ExchangeRateService> _logger,
        [FromKeyedServices(AppConstants.DailyRatesKeyedService)] ICnbRatesCache _dailyRatesCache,
        [FromKeyedServices(AppConstants.MonthlyRatesKeyedService)] ICnbRatesCache _monthlyRatesCache
    ) : IExchangeRateService
    {
        private const string DailyRatesJsonUrl = "/exrates/daily?lang=EN";
        private const string MonthlyRatesJsonUrl = "/fxrates/daily-month?lang=EN&yearMonth={0}";
        private static readonly Currency CZK = new("CZK");

        public async Task<List<Models.ExchangeRate>> GetExchangeRateAsync(IEnumerable<Currency> currencies)
        {
            try
            {
                var dailyRates = await GetDailyRatesAsync();
                var currencyCodes = new HashSet<string>(currencies.Select(c => c.Code), StringComparer.OrdinalIgnoreCase);
                var result = new List<Models.ExchangeRate>();
                var missingCodes = new List<string>();

                foreach (var code in currencyCodes)
                {
                    if (dailyRates.TryGetValue(code, out var value))
                    {
                        result.Add(new Models.ExchangeRate(new Currency(code), CZK, value));
                    }
                    else
                    {
                        missingCodes.Add(code);
                    }
                }

                if (missingCodes.Any())
                {
                    var monthlyRates = await GetMonthlyRatesAsync();
                    foreach (var code in missingCodes)
                    {
                        if (monthlyRates.TryGetValue(code, out var mValue))
                        {
                            result.Add(new Models.ExchangeRate(new Currency(code), CZK, mValue));
                        }
                        else
                        {
                            _logger.LogWarning("Currency {CurrencyCode} not found in CNB daily or monthly data.", code);
                        }
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
                return await _dailyRatesCache.GetOrCreateAsync(async () =>
                {
                    _logger.LogInformation("Fetching daily exchange rates from Exchange rates API.");
                    var rawJson = await _czechApiClient.GetAsync(DailyRatesJsonUrl);
                    return ParseRates(rawJson);
                });
        }

        private async Task<Dictionary<string, decimal>> GetMonthlyRatesAsync()
        {
            var yearMonth = DateTimeExtensions.GetPreviousYearMonthUtc();
                return await _monthlyRatesCache.GetOrCreateAsync(async () =>
                {
                    _logger.LogInformation("Fetching monthly  exchange rates of other countries for {YearMonth}", yearMonth);
                    var rawJson = await _czechApiClient.GetAsync(string.Format(MonthlyRatesJsonUrl, yearMonth));
                    return ParseRates(rawJson);
                });
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
