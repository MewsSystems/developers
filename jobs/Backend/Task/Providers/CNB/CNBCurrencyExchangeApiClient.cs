using ExchangeRateUpdater.Contracts;
using ExchangeRateUpdater.Exceptions;
using ExchangeRateUpdater.Providers.CNB.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Providers.CNB;


internal class CNBExchangeRateApiClient : ICNBExchangeRateApiClient
{
    private static readonly Currency _destinationCurrency = new("CZK");
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly HttpClient _httpClient;
    private readonly ILogger<CNBExchangeRateApiClient> _logger;

    public CNBExchangeRateApiClient(HttpClient httpClient, ILogger<CNBExchangeRateApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IEnumerable<ExchangeRate>> FetchExchangeRatesAsync(DateOnly date, CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/cnbapi/exrates/daily?date={date:yyyy-MM-dd}&lang=EN", ct);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync(ct);

            var cnbExchangeRates = JsonSerializer.Deserialize<CNBExchangeRateResponse>(responseContent, _jsonSerializerOptions)
                ?? throw new ExchangeRateProviderException("Error getting exchange rates from source, bad expected data");

            return cnbExchangeRates.Rates.Select(x =>
                new ExchangeRate(
                    sourceCurrency: new Currency(x.CurrencyCode),
                    targetCurrency: _destinationCurrency,
                    value: x.Rate));
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error fetching exchange rates from CNB for {Date}", date);
            throw new ExchangeRateProviderException("Failed to fetch exchange rates from CNB", ex);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error deserializing CNB response for {Date}", date);
            throw new ExchangeRateProviderException("Invalid response format from CNB", ex);
        }
    }
}
