using ApplicationLayer.DTOs.ExchangeRateProviders;
using REST.Response.Models.Areas.Providers;
using REST.Response.Models.Common;

namespace REST.Response.Converters;

/// <summary>
/// Converters for transforming Provider DTOs to API response models.
/// </summary>
public static class ProviderConverters
{
    /// <summary>
    /// Converts ExchangeRateProviderDto to ProviderResponse.
    /// </summary>
    public static ProviderResponse ToResponse(this ExchangeRateProviderDto dto)
    {
        return new ProviderResponse
        {
            Id = dto.Id,
            Name = dto.Name,
            Code = dto.Code,
            Url = dto.Url,
            BaseCurrency = dto.BaseCurrencyCode,
            RequiresAuthentication = false, // Default value - can be enhanced if needed
            IsActive = dto.IsActive,
            LastSuccessfulFetch = dto.LastSuccessfulFetch,
            LastFailedFetch = dto.LastFailedFetch,
            ConsecutiveFailures = dto.ConsecutiveFailures
        };
    }

    /// <summary>
    /// Converts ProviderHealthDto to ProviderHealthResponse.
    /// </summary>
    public static ProviderHealthResponse ToResponse(this ProviderHealthDto dto)
    {
        return new ProviderHealthResponse
        {
            Provider = new ProviderInfo
            {
                Id = dto.ProviderId,
                Code = dto.ProviderCode,
                Name = dto.ProviderName
            },
            IsActive = true, // Default - may need to be passed separately if needed
            HealthStatus = dto.Status,
            LastSuccessfulFetch = dto.LastSuccessfulFetch,
            LastFailedFetch = dto.LastFailedFetch,
            ConsecutiveFailures = dto.ConsecutiveFailures,
            TotalSuccessfulFetches = 0, // These would need to come from additional data
            TotalFailedFetches = 0,
            AverageFetchDurationMs = null
        };
    }

    /// <summary>
    /// Converts ProviderConfigurationDto to ProviderConfigurationResponse.
    /// </summary>
    public static ProviderConfigurationResponse ToResponse(this ProviderConfigurationDto dto)
    {
        return new ProviderConfigurationResponse
        {
            Id = dto.Id,
            ProviderId = dto.ProviderId,
            SettingKey = dto.SettingKey,
            SettingValue = dto.SettingValue,
            Description = dto.Description,
            Created = dto.Created,
            Modified = dto.Modified
        };
    }

    /// <summary>
    /// Converts ProviderStatisticsDto to ProviderStatisticsResponse.
    /// </summary>
    public static ProviderStatisticsResponse ToResponse(this ProviderStatisticsDto dto)
    {
        return new ProviderStatisticsResponse
        {
            ProviderId = dto.ProviderId,
            ProviderCode = dto.ProviderCode,
            ProviderName = dto.ProviderName,
            TotalRatesProvided = dto.TotalRatesProvided,
            TotalFetchAttempts = dto.TotalFetchAttempts,
            SuccessfulFetches = dto.SuccessfulFetches,
            FailedFetches = dto.FailedFetches,
            SuccessRate = dto.SuccessRate,
            FirstFetchDate = dto.FirstFetchDate,
            LastFetchDate = dto.LastFetchDate,
            AverageFetchInterval = dto.AverageFetchInterval?.ToString(@"d\.hh\:mm\:ss"),
            OldestRateDate = dto.OldestRateDate?.ToString("yyyy-MM-dd"),
            NewestRateDate = dto.NewestRateDate?.ToString("yyyy-MM-dd")
        };
    }

    /// <summary>
    /// Converts ExchangeRateProviderDetailDto to ProviderDetailResponse.
    /// </summary>
    public static ProviderDetailResponse ToResponse(this ExchangeRateProviderDetailDto dto)
    {
        return new ProviderDetailResponse
        {
            Id = dto.Id,
            Name = dto.Name,
            Code = dto.Code,
            Url = dto.Url,
            BaseCurrency = dto.BaseCurrencyCode,
            RequiresAuthentication = dto.RequiresAuthentication,
            ApiKeyVaultReference = dto.ApiKeyVaultReference,
            IsActive = dto.IsActive,
            Status = dto.Status,
            ConsecutiveFailures = dto.ConsecutiveFailures,
            LastSuccessfulFetch = dto.LastSuccessfulFetch,
            LastFailedFetch = dto.LastFailedFetch,
            Created = dto.Created,
            Modified = dto.Modified,
            Configurations = dto.Configurations.Select(c => c.ToResponse()).ToList()
        };
    }

    // ==================== GROUPED CONVERTERS (using GroupBy) ====================

    /// <summary>
    /// Converts a collection of ProviderHealthDtos to grouped response by health status.
    /// Groups providers by their health state (Healthy, Warning, Critical, Inactive).
    /// </summary>
    public static IEnumerable<ProvidersByHealthStatusResponse> ToGroupedByHealthStatusResponse(
        this IEnumerable<ProviderHealthDto> dtos)
    {
        return dtos
            .GroupBy(dto => dto.Status)
            .Select(group => new ProvidersByHealthStatusResponse
            {
                HealthStatus = group.Key,
                Providers = group.Select(dto => new ProviderHealthItem
                {
                    Provider = new ProviderInfo
                    {
                        Id = dto.ProviderId,
                        Code = dto.ProviderCode,
                        Name = dto.ProviderName
                    },
                    IsActive = true, // Default - may need to be passed separately if needed
                    LastSuccessfulFetch = dto.LastSuccessfulFetch,
                    LastFailedFetch = dto.LastFailedFetch,
                    ConsecutiveFailures = dto.ConsecutiveFailures,
                    TotalSuccessfulFetches = 0, // These would need to come from additional data
                    TotalFailedFetches = 0,
                    AverageFetchDurationMs = null
                }).ToList()
            });
    }
}
