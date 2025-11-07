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
}
