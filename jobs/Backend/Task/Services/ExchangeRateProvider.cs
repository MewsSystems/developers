using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System;


public class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly ILogger<ExchangeRateProvider> _logger;
    private readonly HttpClient _httpClient;
    private readonly ICnbXmlParser _parser;
    private readonly string _cnbUrl;

    public ExchangeRateProvider(
        HttpClient httpClient,
        ILogger<ExchangeRateProvider> logger,
        ICnbXmlParser parser,
        IOptions<CnbOptions> options)
    {
        _httpClient = httpClient;
        _logger = logger;
        _parser = parser;
        _cnbUrl = options.Value.DailyUrl;
    }

    /// <summary>
    /// Fetches the daily exchange rates from the Czech National Bank,
    /// filters only the requested currencies, and parses them using ICnbXmlParser.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        if (currencies == null || !currencies.Any())
        {
            _logger.LogWarning("No currencies specified for exchange rate retrieval.");
            return Enumerable.Empty<ExchangeRate>();
        }

        _logger.LogInformation("Fetching exchange rates for {Count} currencies...", currencies.Count());

        string xml;
        try
        {
            xml = await _httpClient.GetStringAsync(_cnbUrl); // TODO: consider implementing retry policy using Polly for transient HTTP errors.
        }
        catch (HttpRequestException ex) // TODO: cache the latest response in memory for a fallback strategy in case of failure.
        {
            _logger.LogError(ex, "HTTP error while fetching exchange rates.");
            return Enumerable.Empty<ExchangeRate>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while fetching exchange rates.");
            return Enumerable.Empty<ExchangeRate>();
        }

        try
        {
            var parsedRates = _parser.Parse(xml, currencies);
            _logger.LogInformation("Successfully parsed {Count} exchange rates.", parsedRates.Count());
            return parsedRates;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parsing exchange rate XML.");
            return Enumerable.Empty<ExchangeRate>();
        }
    }
}
