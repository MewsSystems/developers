using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Common;
using ApplicationLayer.DTOs.SystemHealth;

namespace ApplicationLayer.Queries.SystemHealth.GetFetchActivity;

/// <summary>
/// Query to get recent fetch activity for monitoring provider performance.
/// </summary>
public record GetFetchActivityQuery(
    int Count = 50,
    int? ProviderId = null,
    bool FailedOnly = false) : IQuery<PagedResult<FetchActivityDto>>;
