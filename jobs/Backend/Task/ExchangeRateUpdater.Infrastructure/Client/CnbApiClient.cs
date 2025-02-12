using ExchangeRateUpdater.Application.Exceptions;
using ExchangeRateUpdater.Application.Settings;
using ExchangeRateUpdater.Domain.External;
using ExchangeRateUpdater.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.Clients;

/// <summary>
/// Client for interacting with the Czech National Bank (CNB) API to retrieve exchange rates.
/// </summary>
/// <remarks>
/// The CNB API provides official exchange rate data.
/// Swagger documentation: <see href="https://api.cnb.cz/cnbapi/api-docs.json" />
/// CNB Swagger UI: <see href="https://api.cnb.cz/cnbapi/swagger-ui.html" />.
/// </remarks>
public class CnbApiClient : ICnbApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CnbApiClient> _logger;
    private readonly ApiSettings _apiSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="CnbApiClient"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client used to make requests.</param>
    /// <param name="logger">The logger instance for logging API interactions.</param>
    /// <param name="apiSettings">Configuration settings for the API.</param>
    public CnbApiClient(HttpClient httpClient, ILogger<CnbApiClient> logger, IOptions<ApiSettings> apiSettings)
    {
        _httpClient = httpClient;
        _logger = logger;
        _apiSettings = apiSettings.Value;
    }

    /// <inheritdoc/>
    public async Task<CnbApiResponse?> GetExchangeRatesAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        var queryParams = new KeyValuePair<string, string?>[]
        {
            new ("date", date.ToString("yyyy-MM-dd")),
            new ("lang", "EN")
        };
        var requestUri = BuildUri("cnbapi/exrates/daily", queryParams);

        try
        {
            _logger.LogInformation("Fetching exchange rates from CNB API for date: {Date}", date.ToString("yyyy-MM-dd"));

            var response = await _httpClient.GetAsync(requestUri, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new ExternalServiceException($"API returned {response.StatusCode}: {errorContent}");
            }

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            return JsonSerializer.Deserialize<CnbApiResponse>(responseContent)
                ?? throw new ParsingException("Response was null or empty.");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to reach CNB API.");
            throw new ExternalServiceException("Failed to reach the CNB API.", ex);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse CNB API response.");
            throw new ParsingException("Failed to parse API response.", ex);
        }
    }

    /// <summary>
    /// Builds a complete URI for an API request by appending query parameters.
    /// </summary>
    /// <param name="path">The endpoint path relative to the base URL.</param>
    /// <param name="queryParams">Key-value pairs representing query parameters.</param>
    /// <returns>A <see cref="Uri"/> representing the full request URL.</returns>
    private Uri BuildUri(string path, KeyValuePair<string, string?>[] queryParams)
    {
        var baseUri = new Uri(_apiSettings.BaseUrl);
        var uriBuilder = new UriBuilder(new Uri(baseUri, path))
        {
            Query = QueryString.Create(queryParams).ToUriComponent()
        };

        return uriBuilder.Uri;
    }
}
