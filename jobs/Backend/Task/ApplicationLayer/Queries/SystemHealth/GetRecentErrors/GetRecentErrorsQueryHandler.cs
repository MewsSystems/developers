using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Common;
using ApplicationLayer.DTOs.SystemHealth;
using DomainLayer.Interfaces.Queries;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.SystemHealth.GetRecentErrors;

/// <summary>
/// Handler for retrieving recent error summaries.
/// Uses optimized database view vw_ErrorSummary for performance.
/// </summary>
public class GetRecentErrorsQueryHandler
    : IQueryHandler<GetRecentErrorsQuery, PagedResult<ErrorSummaryDto>>
{
    private readonly ISystemViewQueries _systemViewQueries;
    private readonly ILogger<GetRecentErrorsQueryHandler> _logger;

    public GetRecentErrorsQueryHandler(
        ISystemViewQueries systemViewQueries,
        ILogger<GetRecentErrorsQueryHandler> logger)
    {
        _systemViewQueries = systemViewQueries;
        _logger = logger;
    }

    public async Task<PagedResult<ErrorSummaryDto>> Handle(
        GetRecentErrorsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Getting recent errors from view (Count: {Count}, Severity: {Severity})",
            request.Count, request.Severity);

        try
        {
            // Query optimized database view (24 hours by default)
            var viewResults = await _systemViewQueries.GetErrorSummaryAsync(24, cancellationToken);

            // Apply severity filter if specified
            var filtered = viewResults.AsEnumerable();
            if (!string.IsNullOrEmpty(request.Severity))
            {
                filtered = filtered.Where(e => e.Severity.Equals(request.Severity, StringComparison.OrdinalIgnoreCase));
            }

            // Group errors by error type and message for summary
            var errorsList = filtered.Take(request.Count * 10).ToList(); // Get more for grouping
            var groupedErrors = errorsList
                .GroupBy(e => new { e.Source, e.Message })
                .Select(g => new ErrorSummaryDto
                {
                    ErrorType = g.Key.Source,
                    ErrorMessage = g.Key.Message,
                    OccurrenceCount = g.Count(),
                    FirstOccurrence = g.Min(e => e.Timestamp),
                    LastOccurrence = g.Max(e => e.Timestamp),
                    AffectedProviders = new List<string>() // TODO: Extract from error log data when available
                })
                .OrderByDescending(e => e.LastOccurrence)
                .Take(request.Count)
                .ToList();

            _logger.LogDebug("Retrieved {Count} error summaries from {TotalErrors} errors from view",
                groupedErrors.Count,
                errorsList.Count);

            return PagedResult<ErrorSummaryDto>.Create(
                groupedErrors,
                groupedErrors.Count,
                1,
                request.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving recent errors from view");
            throw;
        }
    }
}
