using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Configuration;

public class ExchangeRateHealthCheck : IHealthCheck
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExchangeRateHealthCheck> _logger;

    public ExchangeRateHealthCheck(IHttpClientFactory httpClientFactory, ILogger<ExchangeRateHealthCheck> logger)
    {
        _httpClient = httpClientFactory.CreateClient(nameof(ExchangeRateService));
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync("exrates/daily", cancellationToken);

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
