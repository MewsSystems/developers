using Common.DTOs;
using Common.Interfaces;
using ConfigurationLayer.Option;
using EuropeanCentralBank.Converters;
using EuropeanCentralBank.Models;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace EuropeanCentralBank;

/// <summary>
/// Exchange rate provider for the European Central Bank (ECB).
/// Fetches daily exchange rates from the ECB public XML API.
/// Configuration is loaded from ExchangeRateProviderOptions.
/// </summary>
public class EuropeanCentralBankProvider : IExchangeRateProvider
{
    private readonly HttpClient _httpClient;
    private readonly EcbConverter _converter;
    private readonly XmlSerializer _xmlSerializer;
    private readonly ExchangeRateProviderOptions _options;
    private readonly bool _shouldDisposeHttpClient;

    /// <summary>
    /// Initializes a new instance of the EuropeanCentralBankProvider.
    /// </summary>
    /// <param name="options">Provider configuration containing URL, base currency, etc.</param>
    /// <param name="httpClient">Optional HttpClient for making API requests. If null, creates a new instance.</param>
    /// <exception cref="ArgumentNullException">Thrown when options is null</exception>
    /// <exception cref="ArgumentException">Thrown when required configuration is missing</exception>
    public EuropeanCentralBankProvider(
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

        _converter = new EcbConverter(_options.BaseCurrency);
        _xmlSerializer = new XmlSerializer(typeof(EcbEnvelope));
    }

    /// <summary>
    /// Fetches the latest (today's) exchange rates from ECB.
    /// Uses URL and base currency from configuration.
    /// </summary>
    /// <returns>
    /// A tuple containing:
    /// - Status: (HTTP status code, status message)
    /// - Exchange rates: List of ExchangeRateDTO with configured base currency (EUR)
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
                var statusMessage = $"{_options.Name}: API request failed with status {statusCode}: {response.ReasonPhrase}";
                return ((statusCode, statusMessage), new List<ExchangeRateDTO>());
            }

            // Read XML content
            var xmlContent = await response.Content.ReadAsStringAsync();

            // Remove namespaces from XML to handle both namespaced and non-namespaced responses
            xmlContent = RemoveXmlNamespaces(xmlContent);

            // Deserialize XML to EcbEnvelope
            using var stringReader = new StringReader(xmlContent);
            var ecbEnvelope = _xmlSerializer.Deserialize(stringReader) as EcbEnvelope;

            if (ecbEnvelope == null)
            {
                return ((500, $"{_options.Name}: Failed to deserialize XML response"), new List<ExchangeRateDTO>());
            }

            // Convert to ExchangeRateDTO list
            var exchangeRates = await _converter.Convert(ecbEnvelope);

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
    /// Fetches historical exchange rates from ECB.
    /// ECB provides multiple historical data endpoints:
    /// - Last 90 days: eurofxref-hist-90d.xml (~100 KB)
    /// - All historical data: eurofxref-hist.xml (~10 MB, large file)
    /// This implementation fetches the 90-day historical data by default.
    /// </summary>
    /// <returns>
    /// A tuple containing:
    /// - Status: (HTTP status code, status message)
    /// - Exchange rates: List of ExchangeRateDTO for the last 90 days
    /// </returns>
    /// <remarks>
    /// The historical endpoints use the same XML structure as the daily endpoint (EcbEnvelope).
    /// The 90-day file contains multiple date cubes, one for each day.
    /// For better performance, consider caching the historical data.
    /// </remarks>
    public async Task<((int, string), List<ExchangeRateDTO>)> GetHistoryExchangeRates()
    {
        // Get historical URL from configuration (prefer 90-day endpoint)
        string? historicalUrl = null;

        if (_options.Configuration != null)
        {
            // Try 90-day endpoint first (smaller, more practical)
            if (_options.Configuration.TryGetValue("Historical90DaysUrl", out var url90Days) &&
                !string.IsNullOrWhiteSpace(url90Days))
            {
                historicalUrl = url90Days;
            }
            // Fallback to full history endpoint if 90-day not configured
            else if (_options.Configuration.TryGetValue("HistoricalAllUrl", out var urlAll) &&
                     !string.IsNullOrWhiteSpace(urlAll))
            {
                historicalUrl = urlAll;
            }
        }

        if (string.IsNullOrWhiteSpace(historicalUrl))
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
            // Fetch XML from historical URL
            var response = await _httpClient.GetAsync(historicalUrl);

            if (!response.IsSuccessStatusCode)
            {
                var statusCode = (int)response.StatusCode;
                var statusMessage = $"{_options.Name}: Historical data request failed with status {statusCode}: {response.ReasonPhrase}";
                return ((statusCode, statusMessage), new List<ExchangeRateDTO>());
            }

            // Read XML content
            var xmlContent = await response.Content.ReadAsStringAsync();

            // Remove namespaces from XML to handle both namespaced and non-namespaced responses
            xmlContent = RemoveXmlNamespaces(xmlContent);

            // Deserialize XML to EcbEnvelope (same structure as daily endpoint)
            using var stringReader = new StringReader(xmlContent);
            var ecbEnvelope = _xmlSerializer.Deserialize(stringReader) as EcbEnvelope;

            if (ecbEnvelope == null)
            {
                return ((500, $"{_options.Name}: Failed to deserialize historical XML response"), new List<ExchangeRateDTO>());
            }

            // Convert to ExchangeRateDTO list (will contain rates from multiple dates)
            var exchangeRates = await _converter.Convert(ecbEnvelope);

            // Determine which endpoint was used for the message
            var dataSource = historicalUrl.Contains("90d") ? "last 90 days" : "all historical data";

            return ((200, $"{_options.Name}: Successfully retrieved {exchangeRates.Count} historical exchange rates ({dataSource})"), exchangeRates);
        }
        catch (HttpRequestException ex)
        {
            return ((503, $"{_options.Name}: Historical data network error - {ex.Message}"), new List<ExchangeRateDTO>());
        }
        catch (TaskCanceledException ex)
        {
            return ((408, $"{_options.Name}: Historical data request timeout - {ex.Message}"), new List<ExchangeRateDTO>());
        }
        catch (InvalidOperationException ex)
        {
            return ((500, $"{_options.Name}: Historical data validation error - {ex.Message}"), new List<ExchangeRateDTO>());
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
