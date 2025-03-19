using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Services;

/// <summary>
/// Health check for the Exchange Rate API.
/// This class verifies that the external exchange rate service is reachable and responding.
/// It uses IHttpClientFactory to create a new HttpClient instance for each check,
/// ensuring proper disposal and efficient connection management.
/// </summary>
public class ExchangeRateHealthCheck : IHealthCheck
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ExchangeRateHealthCheck> _logger;

    /// <summary>
    /// Initialises a new instance of the <see cref="ExchangeRateHealthCheck"/> class.
    /// </summary>
    /// <param name="httpClientFactory">Factory for creating HttpClient instances.</param>
    /// <param name="logger">Logger for logging health check status.</param>
    public ExchangeRateHealthCheck(IHttpClientFactory httpClientFactory, ILogger<ExchangeRateHealthCheck> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    /// <summary>
    /// Performs a health check to verify the availability of the Exchange Rate API.
    /// </summary>
    /// <param name="context">The health check context.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A <see cref="HealthCheckResult"/> indicating the status of the external API.</returns>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using var httpClient = _httpClientFactory.CreateClient(nameof(ExchangeRateService));
            var response = await httpClient.GetAsync("exrates/daily", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Health Check: Exchange rate API is reachable.");
                return HealthCheckResult.Healthy("Exchange rate API is reachable.");
            }
            else
            {
                _logger.LogWarning("Health Check Failed: API returned status code {StatusCode}", response.StatusCode);
                return HealthCheckResult.Unhealthy($"Exchange rate API returned status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Health Check Exception: {Message}", ex.Message);
            return HealthCheckResult.Unhealthy($"Exception when calling exchange rate API: {ex.Message}");
        }
    }
}
