using ExchangeRateUpdater.Core.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NodaTime;

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
            // First try to get the last workday
            var today = _dateTimeProvider.GetCurrentDate();
            var lastWorkingDay = _dateTimeProvider.GetLastWorkingDay(today);

            // Check if CNB API is accessible for the last working day
            var isAvailable = await _dataSource.IsDataAvailableForDateAsync(lastWorkingDay, cancellationToken);

            if (isAvailable) return HealthCheckResult.Healthy($"CNB API is accessible (checked for working day: {lastWorkingDay})");

            // If no result for last workday, try with previous workdays up to 5 days back
            for (var i = 1; i <= 5; i++)
            {
                var previousWorkingDay = _dateTimeProvider.GetLastWorkingDay(lastWorkingDay.Minus(Period.FromDays(1)));

                if (previousWorkingDay == lastWorkingDay)
                    break; // No more working days to check

                lastWorkingDay = previousWorkingDay;
                isAvailable = await _dataSource.IsDataAvailableForDateAsync(lastWorkingDay, cancellationToken);

                if (isAvailable) return HealthCheckResult.Healthy($"CNB API is accessible (checked for previous working day: {lastWorkingDay})");
            }

            return HealthCheckResult.Degraded("CNB API returned no data for recent working days");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CNB API health check failed");
            return HealthCheckResult.Unhealthy("CNB API connection failed", ex);
        }
    }
}