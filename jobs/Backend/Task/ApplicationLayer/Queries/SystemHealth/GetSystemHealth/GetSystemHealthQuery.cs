using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.SystemHealth;

namespace ApplicationLayer.Queries.SystemHealth.GetSystemHealth;

/// <summary>
/// Query to get overall system health and statistics for dashboard.
/// </summary>
public record GetSystemHealthQuery() : IQuery<SystemHealthDto>;
