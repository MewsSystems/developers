using Microsoft.Extensions.Diagnostics.HealthChecks;

public class ExternalApiHealthCheck : IHealthCheck
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public ExternalApiHealthCheck(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var httpClient = _httpClientFactory.CreateClient();
        try
        {
            var response = await httpClient.GetAsync(_configuration["ExchangeRateProvider:BaseUrl"], cancellationToken);
            if (response.IsSuccessStatusCode)
                return HealthCheckResult.Healthy("The CNB API is available.");
            else
                return HealthCheckResult.Unhealthy("The CNB API is not available.");
        }
        catch
        {
            return HealthCheckResult.Unhealthy("The CNB API is not available.");
        }
    }
}
