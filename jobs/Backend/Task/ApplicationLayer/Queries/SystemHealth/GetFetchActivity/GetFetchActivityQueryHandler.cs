using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Common;
using ApplicationLayer.DTOs.SystemHealth;
using DomainLayer.Interfaces.Queries;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.SystemHealth.GetFetchActivity;

/// <summary>
/// Handler for retrieving recent fetch activity.
/// Uses optimized database view vw_RecentFetchActivity for performance.
/// </summary>
public class GetFetchActivityQueryHandler
    : IQueryHandler<GetFetchActivityQuery, PagedResult<FetchActivityDto>>
{
    private readonly ISystemViewQueries _systemViewQueries;
    private readonly ILogger<GetFetchActivityQueryHandler> _logger;

    public GetFetchActivityQueryHandler(
        ISystemViewQueries systemViewQueries,
        ILogger<GetFetchActivityQueryHandler> logger)
    {
        _systemViewQueries = systemViewQueries;
        _logger = logger;
    }

    public async Task<PagedResult<FetchActivityDto>> Handle(
        GetFetchActivityQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug(
            "Getting fetch activity from view (Count: {Count}, ProviderId: {ProviderId}, FailedOnly: {FailedOnly})",
            request.Count,
            request.ProviderId,
            request.FailedOnly);

        try
        {
            // Query optimized database view with generous limit for filtering
            var viewResults = await _systemViewQueries.GetRecentFetchActivityAsync(
                request.Count * 2, // Get more records to account for filtering
                cancellationToken);

            // Apply filters
            var filtered = viewResults.AsEnumerable();

            if (request.FailedOnly)
            {
                filtered = filtered.Where(log => log.Status == "Failed" || log.Status == "Error");
            }

            if (request.ProviderId.HasValue)
            {
                filtered = filtered.Where(log => log.ProviderId == request.ProviderId.Value);
            }

            // Map to DTOs
            var activity = filtered
                .Take(request.Count)
                .Select(log => new FetchActivityDto
                {
                    LogId = log.Id,
                    ProviderId = log.ProviderId,
                    ProviderCode = log.ProviderCode,
                    ProviderName = log.ProviderName,
                    Status = log.Status,
                    RatesImported = log.RatesImported,
                    RatesUpdated = log.RatesUpdated,
                    ErrorMessage = log.ErrorMessage,
                    StartedAt = log.FetchStarted,
                    CompletedAt = log.FetchCompleted,
                    Duration = log.DurationMs.HasValue
                        ? TimeSpan.FromMilliseconds(log.DurationMs.Value)
                        : log.FetchCompleted.HasValue
                            ? log.FetchCompleted.Value - log.FetchStarted
                            : null
                })
                .ToList();

            _logger.LogDebug("Retrieved {Count} fetch activity records from view", activity.Count);

            return PagedResult<FetchActivityDto>.Create(
                activity,
                activity.Count,
                1,
                request.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving fetch activity from view");
            throw;
        }
    }
}
