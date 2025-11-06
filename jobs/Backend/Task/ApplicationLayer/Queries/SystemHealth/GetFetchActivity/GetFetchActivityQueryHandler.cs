using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Common;
using ApplicationLayer.DTOs.SystemHealth;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.SystemHealth.GetFetchActivity;

public class GetFetchActivityQueryHandler
    : IQueryHandler<GetFetchActivityQuery, PagedResult<FetchActivityDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetFetchActivityQueryHandler> _logger;

    public GetFetchActivityQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetFetchActivityQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PagedResult<FetchActivityDto>> Handle(
        GetFetchActivityQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug(
            "Getting fetch activity (Count: {Count}, ProviderId: {ProviderId}, FailedOnly: {FailedOnly})",
            request.Count,
            request.ProviderId,
            request.FailedOnly);

        try
        {
            IEnumerable<DomainLayer.Interfaces.Repositories.FetchLogEntry> logs;

            if (request.FailedOnly)
            {
                logs = await _unitOfWork.FetchLogs.GetFailedLogsAsync(cancellationToken);
            }
            else if (request.ProviderId.HasValue)
            {
                logs = await _unitOfWork.FetchLogs.GetLogsByProviderAsync(
                    request.ProviderId.Value,
                    cancellationToken);
            }
            else
            {
                logs = await _unitOfWork.FetchLogs.GetRecentLogsAsync(
                    request.Count,
                    cancellationToken);
            }

            var activity = logs
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

            _logger.LogDebug("Retrieved {Count} fetch activity records", activity.Count);

            return PagedResult<FetchActivityDto>.Create(
                activity,
                activity.Count,
                1,
                request.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving fetch activity");
            throw;
        }
    }
}
