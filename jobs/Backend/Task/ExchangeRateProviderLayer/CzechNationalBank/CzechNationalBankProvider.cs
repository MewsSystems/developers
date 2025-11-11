using Common.DTOs;
using Common.Interfaces;
using ConfigurationLayer.Option;
using CzechNationalBank.Converters;
using CzechNationalBank.Models;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace CzechNationalBank;

/// <summary>
/// Exchange rate provider for the Czech National Bank (CNB - Česká národní banka).
/// Fetches daily exchange rates from the CNB public XML API.
/// Configuration is loaded from ExchangeRateProviderOptions.
/// </summary>
public class CzechNationalBankProvider : IExchangeRateProvider
{
    private readonly HttpClient _httpClient;
    private readonly CnbConverter _converter;
    private readonly XmlSerializer _xmlSerializer;
    private readonly ExchangeRateProviderOptions _options;
    private readonly bool _shouldDisposeHttpClient;

    /// <summary>
    /// Initializes a new instance of the CzechNationalBankProvider.
    /// </summary>
    /// <param name="options">Provider configuration containing URL, base currency, etc.</param>
    /// <param name="httpClient">Optional HttpClient for making API requests. If null, creates a new instance.</param>
    /// <exception cref="ArgumentNullException">Thrown when options is null</exception>
    /// <exception cref="ArgumentException">Thrown when required configuration is missing</exception>
    public CzechNationalBankProvider(
        ExchangeRateProviderOptions options,
        HttpClient? httpClient = null)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));

        if (string.IsNullOrWhiteSpace(_options.Url))
            throw new ArgumentException("Provider URL is required", nameof(options));

        if (string.IsNullOrWhiteSpace(_options.BaseCurrency))
            throw new ArgumentException("Base currency is required", nameof(options));

        if (string.IsNullOrWhiteSpace(_options.Name))
            throw new ArgumentException("Provider name is required", nameof(options));

        // Track if we created the HttpClient so we know whether to dispose it
        if (httpClient == null)
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
            _shouldDisposeHttpClient = true;
        }
        else
        {
            _httpClient = httpClient;
            _shouldDisposeHttpClient = false;
        }

        _converter = new CnbConverter(_options.BaseCurrency);
        _xmlSerializer = new XmlSerializer(typeof(CnbExchangeRates));
    }

    /// <summary>
    /// Fetches the latest (today's) exchange rates from CNB.
    /// Uses URL and base currency from configuration.
    /// </summary>
    /// <returns>
    /// A tuple containing:
    /// - Status: (HTTP status code, status message)
    /// - Exchange rates: List of ExchangeRateDTO with configured base currency
    /// </returns>
    public async Task<((int, string), List<ExchangeRateDTO>)> GetExchangeRatesForToday()
    {
        try
        {
            // Fetch XML from configured URL
            var response = await _httpClient.GetAsync(_options.Url);

            if (!response.IsSuccessStatusCode)
            {
                var statusCode = (int)response.StatusCode;
                var statusMessage = $"{_options.Name} API request failed with status {statusCode}: {response.ReasonPhrase}";
                return ((statusCode, statusMessage), new List<ExchangeRateDTO>());
            }

            // Read XML content
            var xmlContent = await response.Content.ReadAsStringAsync();

            // Remove namespaces from XML to handle both namespaced and non-namespaced responses
            xmlContent = RemoveXmlNamespaces(xmlContent);

            // Deserialize XML to CnbExchangeRates
            using var stringReader = new StringReader(xmlContent);
            var cnbExchangeRates = _xmlSerializer.Deserialize(stringReader) as CnbExchangeRates;

            if (cnbExchangeRates == null)
            {
                return ((500, $"{_options.Name}: Failed to deserialize XML response"), new List<ExchangeRateDTO>());
            }

            // Convert to ExchangeRateDTO list
            var exchangeRates = await _converter.Convert(cnbExchangeRates);

            return ((200, $"{_options.Name}: Successfully retrieved {exchangeRates.Count} exchange rates"), exchangeRates);
        }
        catch (HttpRequestException ex)
        {
            return ((503, $"{_options.Name}: Network error - {ex.Message}"), new List<ExchangeRateDTO>());
        }
        catch (TaskCanceledException ex)
        {
            return ((408, $"{_options.Name}: Request timeout - {ex.Message}"), new List<ExchangeRateDTO>());
        }
        catch (InvalidOperationException ex)
        {
            return ((500, $"{_options.Name}: Data validation error - {ex.Message}"), new List<ExchangeRateDTO>());
        }
        catch (Exception ex)
        {
            return ((500, $"{_options.Name}: Unexpected error - {ex.Message}"), new List<ExchangeRateDTO>());
        }
    }

    /// <summary>
    /// Fetches historical exchange rates from CNB.
    /// CNB provides historical data via date parameter: https://www.cnb.cz/...?date=DD.MM.YYYY
    /// This implementation fetches the last 30 days of historical data by default.
    /// </summary>
    /// <returns>
    /// A tuple containing:
    /// - Status: (HTTP status code, status message)
    /// - Exchange rates: List of ExchangeRateDTO for the last 30 days
    /// </returns>
    /// <remarks>
    /// The historical endpoint uses the same XML structure as the daily endpoint (CnbExchangeRates).
    /// CNB requires individual API calls for each date, so this fetches multiple days sequentially.
    /// For better performance, consider caching the historical data.
    /// </remarks>
    public async Task<((int, string), List<ExchangeRateDTO>)> GetHistoryExchangeRates()
    {
        // Get historical URL from configuration
        if (_options.Configuration == null ||
            !_options.Configuration.TryGetValue("HistoricalUrl", out var historicalUrlTemplate) ||
            string.IsNullOrWhiteSpace(historicalUrlTemplate))
        {
            // Fallback to today's rates if no historical URL configured
            var fallbackResult = await GetExchangeRatesForToday();
            if (fallbackResult.Item1.Item1 == 200)
            {
                var message = $"{_options.Name}: Historical URL not configured, returning today's rates only";
                return ((200, message), fallbackResult.Item2);
            }
            return fallbackResult;
        }

        try
        {
            var allExchangeRates = new List<ExchangeRateDTO>();
            var daysToFetch = 30; // Fetch last 30 days by default
            var successfulDays = 0;
            var failedDays = 0;

            // Fetch rates for each of the last 30 days
            for (int daysAgo = 0; daysAgo < daysToFetch; daysAgo++)
            {
                var targetDate = DateTime.UtcNow.AddDays(-daysAgo);

                // Skip weekends (CNB doesn't publish rates on weekends)
                if (targetDate.DayOfWeek == DayOfWeek.Saturday || targetDate.DayOfWeek == DayOfWeek.Sunday)
                    continue;

                // Format date as DD.MM.YYYY (CNB format)
                var dateStr = targetDate.ToString("dd.MM.yyyy");
                var historicalUrl = historicalUrlTemplate.Replace("{date}", dateStr);

                try
                {
                    // Fetch XML from historical URL
                    var response = await _httpClient.GetAsync(historicalUrl);

                    if (!response.IsSuccessStatusCode)
                    {
                        failedDays++;
                        continue; // Skip this day and try the next one
                    }

                    // Read XML content
                    var xmlContent = await response.Content.ReadAsStringAsync();

                    // Remove namespaces from XML to handle both namespaced and non-namespaced responses
                    xmlContent = RemoveXmlNamespaces(xmlContent);

                    // Deserialize XML to CnbExchangeRates (same structure as daily endpoint)
                    using var stringReader = new StringReader(xmlContent);
                    var cnbExchangeRates = _xmlSerializer.Deserialize(stringReader) as CnbExchangeRates;

                    if (cnbExchangeRates != null)
                    {
                        // Convert to ExchangeRateDTO list
                        var dailyRates = await _converter.Convert(cnbExchangeRates);
                        allExchangeRates.AddRange(dailyRates);
                        successfulDays++;
                    }
                    else
                    {
                        failedDays++;
                    }
                }
                catch
                {
                    failedDays++;
                    // Continue with next day even if this one fails
                    continue;
                }
            }

            if (allExchangeRates.Count == 0)
            {
                return ((500, $"{_options.Name}: Failed to retrieve any historical exchange rates"), new List<ExchangeRateDTO>());
            }

            var message = $"{_options.Name}: Successfully retrieved {allExchangeRates.Count} historical exchange rates " +
                         $"({successfulDays} days successful, {failedDays} days skipped/failed)";
            return ((200, message), allExchangeRates);
        }
        catch (HttpRequestException ex)
        {
            return ((503, $"{_options.Name}: Historical data network error - {ex.Message}"), new List<ExchangeRateDTO>());
        }
        catch (TaskCanceledException ex)
        {
            return ((408, $"{_options.Name}: Historical data request timeout - {ex.Message}"), new List<ExchangeRateDTO>());
        }
        catch (Exception ex)
        {
            return ((500, $"{_options.Name}: Historical data unexpected error - {ex.Message}"), new List<ExchangeRateDTO>());
        }
    }

    /// <summary>
    /// Removes XML namespaces and namespace prefixes from XML string.
    /// This makes deserialization more robust and handles both namespaced and non-namespaced responses.
    /// </summary>
    private static string RemoveXmlNamespaces(string xml)
    {
        // Remove xmlns declarations
        xml = Regex.Replace(xml, @"\s+xmlns(:\w+)?=""[^""]*""", string.Empty);

        // Remove namespace prefixes from elements (e.g., <ns:element> becomes <element>)
        xml = Regex.Replace(xml, @"<(/?)(\w+:)?(\w+)", "<$1$3");

        return xml;
    }

    /// <summary>
    /// Disposes the HttpClient if it was created internally.
    /// Does not dispose injected HttpClient instances.
    /// </summary>
    public void Dispose()
    {
        if (_shouldDisposeHttpClient)
        {
            _httpClient?.Dispose();
        }
    }
}
