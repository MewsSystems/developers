using ApplicationLayer.DTOs.ExchangeRateProviders;
using SOAP.Models.Providers;

namespace SOAP.Converters;

/// <summary>
/// Converter extensions for provider DTOs to SOAP models.
/// </summary>
public static class ProviderSoapConverters
{
    /// <summary>
    /// Converts an ExchangeRateProviderDto to SOAP model.
    /// </summary>
    public static ProviderSoap ToSoap(this ExchangeRateProviderDto dto)
    {
        return new ProviderSoap
        {
            Id = dto.Id,
            Code = dto.Code,
            Name = dto.Name,
            Url = dto.Url,
            BaseCurrency = dto.BaseCurrencyCode,
            IsActive = dto.IsActive,
            HealthStatus = dto.Status,
            SuccessfulFetchCount = 0, // Not available in ExchangeRateProviderDto
            FailedFetchCount = dto.ConsecutiveFailures,
            LastFetchAttempt = dto.LastFailedFetch?.ToString("yyyy-MM-ddTHH:mm:ssZ") ?? dto.LastSuccessfulFetch?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            LastSuccessfulFetch = dto.LastSuccessfulFetch?.ToString("yyyy-MM-ddTHH:mm:ssZ")
        };
    }

    /// <summary>
    /// Converts an ExchangeRateProviderDetailDto to SOAP model.
    /// </summary>
    public static ProviderSoap ToSoap(this ExchangeRateProviderDetailDto dto)
    {
        return new ProviderSoap
        {
            Id = dto.Id,
            Code = dto.Code,
            Name = dto.Name,
            Url = dto.Url,
            BaseCurrency = dto.BaseCurrencyCode,
            IsActive = dto.IsActive,
            HealthStatus = dto.Status,
            SuccessfulFetchCount = 0, // Not available in ExchangeRateProviderDetailDto
            FailedFetchCount = dto.ConsecutiveFailures,
            LastFetchAttempt = dto.LastFailedFetch?.ToString("yyyy-MM-ddTHH:mm:ssZ") ?? dto.LastSuccessfulFetch?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            LastSuccessfulFetch = dto.LastSuccessfulFetch?.ToString("yyyy-MM-ddTHH:mm:ssZ")
        };
    }

    /// <summary>
    /// Converts a collection of ExchangeRateProviderDto to SOAP models.
    /// </summary>
    public static ProviderSoap[] ToSoap(this IEnumerable<ExchangeRateProviderDto> dtos)
    {
        return dtos.Select(dto => dto.ToSoap()).ToArray();
    }

    /// <summary>
    /// Converts a ProviderHealthDto to SOAP model.
    /// </summary>
    public static ProviderHealthSoap ToSoap(this ProviderHealthDto dto)
    {
        return new ProviderHealthSoap
        {
            ProviderId = dto.ProviderId,
            ProviderCode = dto.ProviderCode,
            ProviderName = dto.ProviderName,
            Status = dto.Status,
            IsHealthy = dto.IsHealthy,
            ConsecutiveFailures = dto.ConsecutiveFailures,
            LastSuccessfulFetch = dto.LastSuccessfulFetch?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            LastFailedFetch = dto.LastFailedFetch?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            TimeSinceLastSuccess = dto.TimeSinceLastSuccess?.ToString(@"d\.hh\:mm\:ss"),
            TimeSinceLastFailure = dto.TimeSinceLastFailure?.ToString(@"d\.hh\:mm\:ss")
        };
    }

    /// <summary>
    /// Converts a ProviderStatisticsDto to SOAP model.
    /// </summary>
    public static ProviderStatisticsSoap ToSoap(this ProviderStatisticsDto dto)
    {
        return new ProviderStatisticsSoap
        {
            ProviderId = dto.ProviderId,
            ProviderCode = dto.ProviderCode,
            ProviderName = dto.ProviderName,
            TotalRatesProvided = dto.TotalRatesProvided,
            TotalFetchAttempts = dto.TotalFetchAttempts,
            SuccessfulFetches = dto.SuccessfulFetches,
            FailedFetches = dto.FailedFetches,
            SuccessRate = dto.SuccessRate,
            FirstFetchDate = dto.FirstFetchDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            LastFetchDate = dto.LastFetchDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            AverageFetchInterval = dto.AverageFetchInterval?.ToString(@"d\.hh\:mm\:ss"),
            OldestRateDate = dto.OldestRateDate?.ToString("yyyy-MM-dd"),
            NewestRateDate = dto.NewestRateDate?.ToString("yyyy-MM-dd")
        };
    }

    /// <summary>
    /// Converts an ExchangeRateProviderDetailDto to ProviderDetailSoap model with full configuration details.
    /// </summary>
    public static ProviderDetailSoap ToDetailSoap(this ExchangeRateProviderDetailDto dto)
    {
        return new ProviderDetailSoap
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
            Modified = dto.Modified
        };
    }
}
