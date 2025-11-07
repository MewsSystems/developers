using ApplicationLayer.DTOs.ExchangeRateProviders;
using gRPC.Protos.Providers;
using Google.Protobuf.WellKnownTypes;

namespace gRPC.Mappers;

/// <summary>
/// Mappers for converting between ApplicationLayer DTOs and gRPC proto messages for providers.
/// </summary>
public static class ProviderMappers
{
    // ============================================================
    // PROVIDER INFO
    // ============================================================

    /// <summary>
    /// Converts ExchangeRateProviderDto to proto ProviderInfo message
    /// </summary>
    public static ProviderInfo ToProtoProviderInfo(ExchangeRateProviderDto dto)
    {
        return new ProviderInfo
        {
            Id = dto.Id,
            Code = dto.Code,
            Name = dto.Name,
            Url = dto.Url,
            IsActive = dto.IsActive,
            BaseCurrencyCode = dto.BaseCurrencyCode,
            Status = dto.Status,
            ConsecutiveFailures = dto.ConsecutiveFailures,
            LastSuccessfulFetch = dto.LastSuccessfulFetch.HasValue
                ? Timestamp.FromDateTimeOffset(dto.LastSuccessfulFetch.Value)
                : null,
            LastFailedFetch = dto.LastFailedFetch.HasValue
                ? Timestamp.FromDateTimeOffset(dto.LastFailedFetch.Value)
                : null,
            Created = Timestamp.FromDateTimeOffset(dto.Created)
        };
    }

    // ============================================================
    // PROVIDER DETAIL
    // ============================================================

    /// <summary>
    /// Converts ExchangeRateProviderDetailDto to proto ProviderDetailInfo message
    /// </summary>
    public static ProviderDetailInfo ToProtoProviderDetailInfo(ExchangeRateProviderDetailDto dto)
    {
        return new ProviderDetailInfo
        {
            Id = dto.Id,
            Code = dto.Code,
            Name = dto.Name,
            Url = dto.Url,
            IsActive = dto.IsActive,
            BaseCurrencyCode = dto.BaseCurrencyCode,
            RequiresAuthentication = dto.RequiresAuthentication,
            ApiKeyVaultReference = dto.ApiKeyVaultReference ?? "",
            Status = dto.Status,
            ConsecutiveFailures = dto.ConsecutiveFailures,
            LastSuccessfulFetch = dto.LastSuccessfulFetch.HasValue
                ? Timestamp.FromDateTimeOffset(dto.LastSuccessfulFetch.Value)
                : null,
            LastFailedFetch = dto.LastFailedFetch.HasValue
                ? Timestamp.FromDateTimeOffset(dto.LastFailedFetch.Value)
                : null,
            Created = Timestamp.FromDateTimeOffset(dto.Created),
            Modified = dto.Modified.HasValue
                ? Timestamp.FromDateTimeOffset(dto.Modified.Value)
                : null
        };
    }

    // ============================================================
    // PROVIDER HEALTH
    // ============================================================

    /// <summary>
    /// Converts ProviderHealthDto to proto ProviderHealthInfo message
    /// </summary>
    public static ProviderHealthInfo ToProtoProviderHealthInfo(ProviderHealthDto dto)
    {
        return new ProviderHealthInfo
        {
            ProviderId = dto.ProviderId,
            ProviderCode = dto.ProviderCode,
            ProviderName = dto.ProviderName,
            HealthStatus = dto.Status,
            ConsecutiveFailures = dto.ConsecutiveFailures,
            LastSuccessfulFetch = dto.LastSuccessfulFetch.HasValue
                ? Timestamp.FromDateTimeOffset(dto.LastSuccessfulFetch.Value)
                : null,
            LastFailedFetch = dto.LastFailedFetch.HasValue
                ? Timestamp.FromDateTimeOffset(dto.LastFailedFetch.Value)
                : null,
            LastErrorMessage = "" // Not available in DTO, but proto expects it
        };
    }

    // ============================================================
    // PROVIDER STATISTICS
    // ============================================================

    /// <summary>
    /// Converts ProviderStatisticsDto to proto ProviderStatisticsInfo message
    /// </summary>
    public static ProviderStatisticsInfo ToProtoProviderStatisticsInfo(ProviderStatisticsDto dto)
    {
        return new ProviderStatisticsInfo
        {
            ProviderId = dto.ProviderId,
            ProviderCode = dto.ProviderCode,
            ProviderName = dto.ProviderName,
            TotalRatesProvided = dto.TotalRatesProvided,
            TotalFetchAttempts = dto.TotalFetchAttempts,
            SuccessfulFetches = dto.SuccessfulFetches,
            FailedFetches = dto.FailedFetches,
            SuccessRate = (double)dto.SuccessRate,
            FirstFetchDate = dto.FirstFetchDate.HasValue
                ? Timestamp.FromDateTimeOffset(dto.FirstFetchDate.Value)
                : null,
            LastFetchDate = dto.LastFetchDate.HasValue
                ? Timestamp.FromDateTimeOffset(dto.LastFetchDate.Value)
                : null,
            OldestRateDate = dto.OldestRateDate.HasValue
                ? ExchangeRateMappers.ToProtoDate(dto.OldestRateDate.Value)
                : null,
            NewestRateDate = dto.NewestRateDate.HasValue
                ? ExchangeRateMappers.ToProtoDate(dto.NewestRateDate.Value)
                : null
        };
    }
}
