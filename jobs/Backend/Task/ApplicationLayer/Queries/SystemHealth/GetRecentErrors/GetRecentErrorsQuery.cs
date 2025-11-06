using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Common;
using ApplicationLayer.DTOs.SystemHealth;

namespace ApplicationLayer.Queries.SystemHealth.GetRecentErrors;

/// <summary>
/// Query to get recent error logs for monitoring and troubleshooting.
/// </summary>
public record GetRecentErrorsQuery(
    int Count = 50,
    string? Severity = null) : IQuery<PagedResult<ErrorSummaryDto>>;
