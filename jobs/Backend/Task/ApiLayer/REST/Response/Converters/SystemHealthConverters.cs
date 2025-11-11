using ApplicationLayer.DTOs.SystemHealth;
using REST.Response.Models.Areas.SystemHealth;
using REST.Response.Models.Common;

namespace REST.Response.Converters;

/// <summary>
/// Converters for transforming SystemHealth DTOs to API response models.
/// </summary>
public static class SystemHealthConverters
{
    /// <summary>
    /// Converts SystemHealthDto to HealthCheckResponse.
    /// Note: Some fields may need to be calculated or passed separately.
    /// </summary>
    public static HealthCheckResponse ToResponse(this SystemHealthDto dto)
    {
        var status = DetermineHealthStatus(dto);

        return new HealthCheckResponse
        {
            Status = status,
            Timestamp = DateTimeOffset.UtcNow,
            ActiveProviders = dto.ActiveProviders,
            InactiveProviders = dto.TotalProviders - dto.ActiveProviders,
            ProvidersWithFailures = dto.QuarantinedProviders,
            TotalExchangeRates = dto.TotalExchangeRates,
            MostRecentRateUpdate = dto.LatestRateDate.HasValue
                ? new DateTimeOffset(dto.LatestRateDate.Value.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero)
                : null,
            RecentErrorCount = dto.FailedFetchesToday,
            DatabaseStatus = "Connected", // Would need to be determined from actual DB check
            BackgroundJobsStatus = "Running" // Would need to be determined from actual job status
        };
    }

    /// <summary>
    /// Converts ErrorSummaryDto to ErrorSummaryResponse.
    /// Note: The DTOs have different structures - this provides best-effort mapping.
    /// </summary>
    public static ErrorSummaryResponse ToResponse(this ErrorSummaryDto dto)
    {
        return new ErrorSummaryResponse
        {
            Id = 0, // DTO doesn't have ID - would need to be provided separately
            Timestamp = dto.LastOccurrence,
            Severity = DetermineSeverity(dto.ErrorType),
            Source = string.Join(", ", dto.AffectedProviders),
            Message = $"{dto.ErrorMessage} (Count: {dto.OccurrenceCount})",
            Exception = dto.ErrorType,
            UserId = null
        };
    }

    /// <summary>
    /// Converts FetchActivityDto to FetchActivityResponse.
    /// </summary>
    public static FetchActivityResponse ToResponse(this FetchActivityDto dto)
    {
        return new FetchActivityResponse
        {
            Id = dto.LogId,
            Provider = new ProviderInfo
            {
                Id = dto.ProviderId,
                Code = dto.ProviderCode,
                Name = dto.ProviderName
            },
            FetchStarted = dto.StartedAt,
            FetchCompleted = dto.CompletedAt,
            Status = dto.Status,
            RatesImported = dto.RatesImported,
            RatesUpdated = dto.RatesUpdated,
            ErrorMessage = dto.ErrorMessage,
            DurationMs = dto.Duration.HasValue ? (int)dto.Duration.Value.TotalMilliseconds : null
        };
    }

    /// <summary>
    /// Determines overall system health status based on metrics.
    /// </summary>
    private static string DetermineHealthStatus(SystemHealthDto dto)
    {
        // If more than 50% of providers are quarantined, system is unhealthy
        if (dto.TotalProviders > 0 && dto.QuarantinedProviders > dto.TotalProviders / 2)
            return "Unhealthy";

        // If there are some quarantined providers or failures, system is degraded
        if (dto.QuarantinedProviders > 0 || dto.FailedFetchesToday > dto.SuccessfulFetchesToday / 2)
            return "Degraded";

        // Otherwise, system is healthy
        return "Healthy";
    }

    /// <summary>
    /// Determines error severity based on error type.
    /// </summary>
    private static string DetermineSeverity(string errorType)
    {
        // Simple heuristic - could be enhanced based on actual error types
        if (errorType.Contains("Critical", StringComparison.OrdinalIgnoreCase) ||
            errorType.Contains("Fatal", StringComparison.OrdinalIgnoreCase))
            return "Critical";

        if (errorType.Contains("Error", StringComparison.OrdinalIgnoreCase))
            return "Error";

        if (errorType.Contains("Warning", StringComparison.OrdinalIgnoreCase))
            return "Warning";

        return "Info";
    }

    // ==================== GROUPED CONVERTERS (using GroupBy) ====================

    /// <summary>
    /// Converts a collection of FetchActivityDtos to grouped response by provider.
    /// Uses GroupBy to avoid duplicating provider information.
    /// </summary>
    public static IEnumerable<FetchActivitiesByProviderResponse> ToGroupedByProviderResponse(
        this IEnumerable<FetchActivityDto> dtos)
    {
        return dtos
            .GroupBy(dto => new { dto.ProviderId, dto.ProviderCode, dto.ProviderName })
            .Select(group => new FetchActivitiesByProviderResponse
            {
                Provider = new ProviderInfo
                {
                    Id = group.Key.ProviderId,
                    Code = group.Key.ProviderCode,
                    Name = group.Key.ProviderName
                },
                Activities = group.Select(dto => new FetchActivityItem
                {
                    Id = dto.LogId,
                    FetchStarted = dto.StartedAt,
                    FetchCompleted = dto.CompletedAt,
                    Status = dto.Status,
                    RatesImported = dto.RatesImported,
                    RatesUpdated = dto.RatesUpdated,
                    ErrorMessage = dto.ErrorMessage,
                    DurationMs = dto.Duration.HasValue ? (int)dto.Duration.Value.TotalMilliseconds : null
                }).ToList()
            });
    }

    /// <summary>
    /// Converts a collection of FetchActivityDtos to grouped response by status.
    /// Groups activities by their outcome (Success, Failed, Running).
    /// </summary>
    public static IEnumerable<FetchActivitiesByStatusResponse> ToGroupedByStatusResponse(
        this IEnumerable<FetchActivityDto> dtos)
    {
        return dtos
            .GroupBy(dto => dto.Status)
            .Select(group => new FetchActivitiesByStatusResponse
            {
                Status = group.Key,
                Activities = group.Select(dto => new FetchActivityWithProviderItem
                {
                    Id = dto.LogId,
                    Provider = new ProviderInfo
                    {
                        Id = dto.ProviderId,
                        Code = dto.ProviderCode,
                        Name = dto.ProviderName
                    },
                    FetchStarted = dto.StartedAt,
                    FetchCompleted = dto.CompletedAt,
                    RatesImported = dto.RatesImported,
                    RatesUpdated = dto.RatesUpdated,
                    ErrorMessage = dto.ErrorMessage,
                    DurationMs = dto.Duration.HasValue ? (int)dto.Duration.Value.TotalMilliseconds : null
                }).ToList()
            });
    }
}
