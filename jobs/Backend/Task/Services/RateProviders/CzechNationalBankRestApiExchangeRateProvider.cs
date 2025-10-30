using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ExchangeRateUpdater.Config;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Services.RateProviders;

/// <summary>
///     Gets exchange rates from the REST API provided by the Czech National Bank
/// </summary>
public class CzechNationalBankRestApiExchangeRateProvider : IExchangeRateProvider
{
    private readonly AppConfiguration _appConfiguration;
    private readonly ILogger<CzechNationalBankRestApiExchangeRateProvider> _logger;

    public CzechNationalBankRestApiExchangeRateProvider(ILogger<CzechNationalBankRestApiExchangeRateProvider> logger,
        AppConfiguration appConfiguration)
    {
        _logger = logger;
        _appConfiguration = appConfiguration;
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        using var httpClient = new HttpClient();
        var currentDate = DateTime.Now.ToString("yyyy-MM-dd");

        var response =
            await httpClient.GetStringAsync(
                $"{_appConfiguration.DailyRateUrl}/cnbapi/exrates/daily?date={currentDate}&lang=EN");
        var dto = JsonSerializer.Deserialize<ExchangeRateResponseDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (dto?.Rates == null)
        {
            _logger.LogWarning("No exchange rates found in API response");
            return Enumerable.Empty<ExchangeRate>();
        }

        var currencyCodesToFilter = new HashSet<string>(
            currencies.Select(c => c.Code),
            StringComparer.OrdinalIgnoreCase);

        return dto.Rates
            .Where(r => currencyCodesToFilter.Contains(r.CurrencyCode))
            .Select(r => new ExchangeRate(
                new Currency(r.CurrencyCode),
                new Currency(_appConfiguration.CzkCurrencyCode),
                DateTime.Parse(r.ValidFor),
                r.Rate / r.Amount));
    }
}