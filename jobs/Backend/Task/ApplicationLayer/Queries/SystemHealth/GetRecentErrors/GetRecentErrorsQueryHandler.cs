using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Common;
using ApplicationLayer.DTOs.SystemHealth;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.SystemHealth.GetRecentErrors;

public class GetRecentErrorsQueryHandler
    : IQueryHandler<GetRecentErrorsQuery, PagedResult<ErrorSummaryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetRecentErrorsQueryHandler> _logger;

    public GetRecentErrorsQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetRecentErrorsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PagedResult<ErrorSummaryDto>> Handle(
        GetRecentErrorsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Getting recent errors (Count: {Count}, Severity: {Severity})", request.Count, request.Severity);

        try
        {
            IEnumerable<DomainLayer.Interfaces.Repositories.ErrorLogEntry> errors;

            if (!string.IsNullOrEmpty(request.Severity))
            {
                errors = await _unitOfWork.ErrorLogs.GetErrorsBySeverityAsync(
                    request.Severity,
                    cancellationToken);
            }
            else
            {
                errors = await _unitOfWork.ErrorLogs.GetRecentErrorsAsync(
                    request.Count,
                    cancellationToken);
            }

            // Group errors by error type and message for summary
            var errorsList = errors.Take(request.Count).ToList();
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
                .ToList();

            _logger.LogDebug("Retrieved {Count} error summaries from {TotalErrors} errors",
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
            _logger.LogError(ex, "Error retrieving recent errors");
            throw;
        }
    }
}
