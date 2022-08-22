using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using ExchangeRateUpdater.Helpers;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Parsers;
using ExchangeRateUpdater.Settings;

namespace ExchangeRateUpdater.Services;

public class CzechBankExchangeRateService : IExchangeRateService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CzechBankExchangeRateService> _logger;
    private readonly ICurrencyParser _currencyParser;
    private readonly string _czechBankUri;

    public CzechBankExchangeRateService(
        HttpClient httpClient,
        IOptions<AppSettings> options,
        ICurrencyParser currencyParser,
        ILogger<CzechBankExchangeRateService> logger)
    {
        ArgumentsHelper.ThrowIfNull("httpClient", httpClient);
        ArgumentsHelper.ThrowIfNull("options.Value.CzechBankUri", options?.Value.CzechBankUri);
        ArgumentsHelper.ThrowIfNull("currencyParser", currencyParser);
        ArgumentsHelper.ThrowIfNull("logger", logger);

        _httpClient = httpClient;
        _logger = logger;
        _currencyParser = currencyParser;
        _czechBankUri = options?.Value.CzechBankUri;
    }

    /// <summary>
    /// Get foreign exchange market rates for Czech National Bank
    /// </summary>
    /// <param name="date">Date of the exchange rate, without parameter, only for current exchange rates</param>
    public async Task<IReadOnlyCollection<Currency>> GetCurrenciesAsync(DateTime? date = null)
    {
        var dateParameter = date != null ? $"?date={ date:dd.MM.yyyy}" : null;
        var response = await _httpClient.GetAsync($"{_czechBankUri}{dateParameter}");

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"Failed to get exchange rates. Status code: {response.StatusCode}. Message: {response.RequestMessage}");
            return null;
        }

        var content = await response.Content.ReadAsStringAsync();

        return string.IsNullOrEmpty(content) ? null : _currencyParser.ParseCurrencies(content);
    }
}
