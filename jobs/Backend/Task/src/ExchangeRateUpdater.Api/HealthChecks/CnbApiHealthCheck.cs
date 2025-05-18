using ExchangeRateUpdater.Core.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NodaTime;

namespace ExchangeRateUpdater.Api.HealthChecks;

public class CnbApiHealthCheck : IHealthCheck
{
    private readonly IExchangeRateDataSource _dataSource;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<CnbApiHealthCheck> _logger;

    public CnbApiHealthCheck(
        IExchangeRateDataSource dataSource,
        IDateTimeProvider dateTimeProvider,
        ILogger<CnbApiHealthCheck> logger)
    {
        _dataSource = dataSource;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

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