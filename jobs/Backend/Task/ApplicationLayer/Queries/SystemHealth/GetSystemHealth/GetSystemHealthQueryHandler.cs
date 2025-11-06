using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.SystemHealth;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.SystemHealth.GetSystemHealth;

/// <summary>
/// Handler for GetSystemHealthQuery - aggregates system-wide statistics.
/// TODO: Complete when full repository adapters are implemented in InfrastructureLayer.
/// Currently returns limited statistics based on available logging repositories.
/// </summary>
public class GetSystemHealthQueryHandler
    : IQueryHandler<GetSystemHealthQuery, SystemHealthDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetSystemHealthQueryHandler> _logger;

    public GetSystemHealthQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetSystemHealthQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<SystemHealthDto> Handle(
        GetSystemHealthQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Getting system health statistics");

        try
        {
            // Get today's fetch statistics from logs
            var todayStart = DateTimeOffset.UtcNow.Date;
            var todayEnd = todayStart.AddDays(1);
            var todayLogs = await _unitOfWork.FetchLogs.GetLogsByDateRangeAsync(
                todayStart,
                todayEnd,
                cancellationToken);

            var todayLogsList = todayLogs.ToList();
            var totalFetchesToday = todayLogsList.Count;
            var successfulFetchesToday = todayLogsList.Count(l => l.Status == "Success" || l.Status == "Completed");
            var failedFetchesToday = todayLogsList.Count(l => l.Status == "Failed");

            var successRateToday = totalFetchesToday > 0
                ? (decimal)successfulFetchesToday / totalFetchesToday * 100
                : 0;

            // TODO: Add provider, currency, and exchange rate statistics when repository adapters are implemented
            // For now, return placeholder values
            _logger.LogDebug(
                "System health: {TotalFetchesToday} fetches today ({SuccessfulFetches} successful)",
                totalFetchesToday,
                successfulFetchesToday);

            return new SystemHealthDto
            {
                TotalProviders = 0, // TODO: Implement when ExchangeRateProviders repository adapter is ready
                ActiveProviders = 0, // TODO: Implement when ExchangeRateProviders repository adapter is ready
                QuarantinedProviders = 0, // TODO: Implement when ExchangeRateProviders repository adapter is ready
                TotalCurrencies = 0, // TODO: Implement when Currencies repository adapter is ready
                TotalExchangeRates = 0, // TODO: Implement when ExchangeRates repository adapter is ready
                LatestRateDate = null, // TODO: Implement when ExchangeRates repository adapter is ready
                OldestRateDate = null, // TODO: Implement when ExchangeRates repository adapter is ready
                TotalFetchesToday = totalFetchesToday,
                SuccessfulFetchesToday = successfulFetchesToday,
                FailedFetchesToday = failedFetchesToday,
                SuccessRateToday = successRateToday,
                LastUpdated = DateTimeOffset.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting system health");
            throw;
        }
    }
}
