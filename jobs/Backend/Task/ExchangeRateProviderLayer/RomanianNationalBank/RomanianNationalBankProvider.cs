using Common.DTOs;
using Common.Interfaces;
using ConfigurationLayer.Option;
using RomanianNationalBank.Converters;
using RomanianNationalBank.Models;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace RomanianNationalBank;

/// <summary>
/// Exchange rate provider for the Romanian National Bank (BNR - Banca Națională a României).
/// Fetches daily exchange rates from the BNR public XML API.
/// Configuration is loaded from ExchangeRateProviderOptions.
/// </summary>
public class RomanianNationalBankProvider : IExchangeRateProvider
{
    private readonly HttpClient _httpClient;
    private readonly BnrConverter _converter;
    private readonly XmlSerializer _xmlSerializer;
    private readonly ExchangeRateProviderOptions _options;
    private readonly bool _shouldDisposeHttpClient;

    /// <summary>
    /// Initializes a new instance of the RomanianNationalBankProvider.
    /// </summary>
    /// <param name="options">Provider configuration containing URL, base currency, etc.</param>
    /// <param name="httpClient">Optional HttpClient for making API requests. If null, creates a new instance.</param>
    /// <exception cref="ArgumentNullException">Thrown when options is null</exception>
    /// <exception cref="ArgumentException">Thrown when required configuration is missing</exception>
    public RomanianNationalBankProvider(
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

        _converter = new BnrConverter(_options.BaseCurrency);
        _xmlSerializer = new XmlSerializer(typeof(BnrDataSet));
    }

    /// <summary>
    /// Fetches the latest (today's) exchange rates from BNR.
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

            // Deserialize XML to BnrDataSet
            using var stringReader = new StringReader(xmlContent);
            var bnrDataSet = _xmlSerializer.Deserialize(stringReader) as BnrDataSet;

            if (bnrDataSet == null)
            {
                return ((500, $"{_options.Name}: Failed to deserialize XML response"), new List<ExchangeRateDTO>());
            }

            // Convert to ExchangeRateDTO list
            var exchangeRates = await _converter.Convert(bnrDataSet);

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
    /// Fetches historical exchange rates from BNR.
    /// BNR provides historical data by year at: https://www.bnr.ro/files/xml/years/nbrfxrates{year}.xml
    /// This implementation fetches the current year's historical data.
    /// </summary>
    /// <returns>
    /// A tuple containing:
    /// - Status: (HTTP status code, status message)
    /// - Exchange rates: List of ExchangeRateDTO for the current year
    /// </returns>
    /// <remarks>
    /// The historical endpoint uses the same XML structure as the daily endpoint (BnrDataSet).
    /// For better performance, consider caching the yearly data as it's quite large.
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
            // Fetch current year's historical data
            var currentYear = DateTime.UtcNow.Year;
            var historicalUrl = historicalUrlTemplate.Replace("{year}", currentYear.ToString());

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

            // Deserialize XML to BnrDataSet (same structure as daily endpoint)
            using var stringReader = new StringReader(xmlContent);
            var bnrDataSet = _xmlSerializer.Deserialize(stringReader) as BnrDataSet;

            if (bnrDataSet == null)
            {
                return ((500, $"{_options.Name}: Failed to deserialize historical XML response"), new List<ExchangeRateDTO>());
            }

            // Convert to ExchangeRateDTO list
            var exchangeRates = await _converter.Convert(bnrDataSet);

            return ((200, $"{_options.Name}: Successfully retrieved {exchangeRates.Count} historical exchange rates for year {currentYear}"), exchangeRates);
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

        // Remove namespace-prefixed attributes (e.g., xsi:schemaLocation)
        xml = Regex.Replace(xml, @"\s+\w+:\w+=""[^""]*""", string.Empty);

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
