using ExchangeRateUpdater.Core.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ExchangeRateUpdater.Api.HealthChecks;

/// <summary>
/// Health check implementation for the Czech National Bank (CNB) API
/// </summary>
public class CnbApiHealthCheck : IHealthCheck
{
    private readonly IExchangeRateDataSource _dataSource;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<CnbApiHealthCheck> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CnbApiHealthCheck"/> class.
    /// </summary>
    /// <param name="dataSource">The CNB exchange rate data source</param>
    /// <param name="dateTimeProvider">The date/time provider for determining working days</param>
    /// <param name="logger">The logger instance for logging health check operations</param>
    public CnbApiHealthCheck(
        IExchangeRateDataSource dataSource,
        IDateTimeProvider dateTimeProvider,
        ILogger<CnbApiHealthCheck> logger)
    {
        _dataSource = dataSource;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    /// <summary>
    /// Checks the health of the CNB API by attempting to retrieve exchange rate data
    /// </summary>
    /// <param name="context">The health check context</param>
    /// <param name="cancellationToken">A token that can be used to cancel the health check</param>
    /// <returns>A task that represents the asynchronous health check operation</returns>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var today = _dateTimeProvider.GetCurrentDate();

            var isAvailable = await _dataSource.IsDataAvailableForDateAsync(today, cancellationToken);

            if (isAvailable)
                return HealthCheckResult.Healthy($"CNB API is accessible (checked for date: {today})");

            return HealthCheckResult.Degraded($"CNB API returned no data for date: {today}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CNB API health check failed");
            return HealthCheckResult.Unhealthy("CNB API connection failed", ex);
        }
    }
}