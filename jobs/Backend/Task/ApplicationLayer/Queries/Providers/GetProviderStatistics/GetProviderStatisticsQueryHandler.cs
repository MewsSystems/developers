using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.ExchangeRateProviders;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.Providers.GetProviderStatistics;

public class GetProviderStatisticsQueryHandler
    : IQueryHandler<GetProviderStatisticsQuery, ProviderStatisticsDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetProviderStatisticsQueryHandler> _logger;

    public GetProviderStatisticsQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetProviderStatisticsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ProviderStatisticsDto?> Handle(
        GetProviderStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        var provider = await _unitOfWork.ExchangeRateProviders
            .GetByIdAsync(request.ProviderId, cancellationToken);

        if (provider == null)
        {
            _logger.LogWarning("Provider {ProviderId} not found", request.ProviderId);
            return null;
        }

        // Get all exchange rates for this provider to calculate statistics
        // Using optimized single-query method to avoid N+1 problem
        var startDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1)); // Last year
        var endDate = DateOnly.FromDateTime(DateTime.UtcNow);

        // Single database query to get all rates for this provider in the date range
        var allRatesForProvider = (await _unitOfWork.ExchangeRates.GetByProviderAndDateRangeAsync(
            provider.Id,
            startDate,
            endDate,
            cancellationToken)).ToList();

        var totalRates = allRatesForProvider.Count;
        var oldestRateDate = allRatesForProvider.Any() ? allRatesForProvider.Min(r => r.ValidDate) : (DateOnly?)null;
        var newestRateDate = allRatesForProvider.Any() ? allRatesForProvider.Max(r => r.ValidDate) : (DateOnly?)null;

        // Calculate fetch statistics based on provider's tracking
        var totalFetchAttempts = 0;
        var successfulFetches = 0;
        var failedFetches = 0;

        // Estimate based on provider's current state
        if (provider.LastSuccessfulFetch.HasValue || provider.LastFailedFetch.HasValue)
        {
            // This is a rough estimate - in production you'd track this in a separate table
            var daysSinceCreation = (DateTimeOffset.UtcNow - provider.Created).Days;
            totalFetchAttempts = Math.Max(daysSinceCreation, 1); // Assuming ~1 fetch per day

            // Use consecutive failures as indicator
            failedFetches = provider.ConsecutiveFailures;
            successfulFetches = totalFetchAttempts - failedFetches;
        }

        var successRate = totalFetchAttempts > 0
            ? (decimal)successfulFetches / totalFetchAttempts * 100
            : 0;

        var firstFetchDate = provider.LastSuccessfulFetch ?? provider.Created;
        var lastFetchDate = provider.LastSuccessfulFetch ?? provider.LastFailedFetch;

        TimeSpan? averageFetchInterval = null;
        if (lastFetchDate.HasValue)
        {
            var totalDuration = lastFetchDate.Value - firstFetchDate;
            if (totalFetchAttempts > 1)
            {
                averageFetchInterval = TimeSpan.FromTicks(totalDuration.Ticks / (totalFetchAttempts - 1));
            }
        }

        _logger.LogInformation(
            "Retrieved statistics for provider {ProviderCode}: {TotalRates} rates, {SuccessRate}% success rate",
            provider.Code,
            totalRates,
            successRate);

        return new ProviderStatisticsDto
        {
            ProviderId = provider.Id,
            ProviderCode = provider.Code,
            ProviderName = provider.Name,
            TotalRatesProvided = totalRates,
            TotalFetchAttempts = totalFetchAttempts,
            SuccessfulFetches = successfulFetches,
            FailedFetches = failedFetches,
            SuccessRate = Math.Round(successRate, 2),
            FirstFetchDate = firstFetchDate,
            LastFetchDate = lastFetchDate,
            AverageFetchInterval = averageFetchInterval,
            OldestRateDate = oldestRateDate,
            NewestRateDate = newestRateDate
        };
    }
}
