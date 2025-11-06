using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.SystemHealth;
using DomainLayer.Interfaces.Queries;
using DomainLayer.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.SystemHealth.GetSystemHealth;

/// <summary>
/// Handler for GetSystemHealthQuery - aggregates system-wide statistics.
/// Uses optimized database view vw_SystemHealthDashboard for comprehensive metrics.
/// </summary>
public class GetSystemHealthQueryHandler
    : IQueryHandler<GetSystemHealthQuery, SystemHealthDto>
{
    private readonly ISystemViewQueries _systemViewQueries;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<GetSystemHealthQueryHandler> _logger;

    public GetSystemHealthQueryHandler(
        ISystemViewQueries systemViewQueries,
        IDateTimeProvider dateTimeProvider,
        ILogger<GetSystemHealthQueryHandler> logger)
    {
        _systemViewQueries = systemViewQueries;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<SystemHealthDto> Handle(
        GetSystemHealthQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Getting system health statistics from view");

        try
        {
            // Query optimized database view (returns metrics as key-value pairs)
            var metrics = await _systemViewQueries.GetSystemHealthDashboardAsync(cancellationToken);
            var metricDict = metrics.ToDictionary(m => m.Metric, m => m.Value);

            // Helper function to safely parse metric values
            int ParseInt(string metricName, int defaultValue = 0)
            {
                return metricDict.TryGetValue(metricName, out var value) && int.TryParse(value, out var parsed)
                    ? parsed
                    : defaultValue;
            }

            decimal ParseDecimal(string metricName, decimal defaultValue = 0m)
            {
                return metricDict.TryGetValue(metricName, out var value) && decimal.TryParse(value, out var parsed)
                    ? parsed
                    : defaultValue;
            }

            DateOnly? ParseDateOnly(string metricName)
            {
                return metricDict.TryGetValue(metricName, out var value) && DateOnly.TryParse(value, out var parsed)
                    ? parsed
                    : null;
            }

            var result = new SystemHealthDto
            {
                TotalProviders = ParseInt("TotalProviders"),
                ActiveProviders = ParseInt("ActiveProviders"),
                QuarantinedProviders = ParseInt("QuarantinedProviders"),
                TotalCurrencies = ParseInt("TotalCurrencies"),
                TotalExchangeRates = ParseInt("TotalExchangeRates"),
                LatestRateDate = ParseDateOnly("LatestRateDate"),
                OldestRateDate = ParseDateOnly("OldestRateDate"),
                TotalFetchesToday = ParseInt("TotalFetchesToday"),
                SuccessfulFetchesToday = ParseInt("SuccessfulFetchesToday"),
                FailedFetchesToday = ParseInt("FailedFetchesToday"),
                SuccessRateToday = ParseDecimal("SuccessRateToday"),
                LastUpdated = _dateTimeProvider.UtcNow
            };

            _logger.LogDebug(
                "System health from view: {TotalProviders} providers, {TotalCurrencies} currencies, {TotalExchangeRates} rates",
                result.TotalProviders,
                result.TotalCurrencies,
                result.TotalExchangeRates);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting system health from view");
            throw;
        }
    }
}
