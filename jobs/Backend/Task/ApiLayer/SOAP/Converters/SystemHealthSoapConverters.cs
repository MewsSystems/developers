using ApplicationLayer.DTOs.SystemHealth;
using SOAP.Models.SystemHealth;

namespace SOAP.Converters;

/// <summary>
/// Converter extensions for system health DTOs to SOAP models.
/// </summary>
public static class SystemHealthSoapConverters
{
    /// <summary>
    /// Converts a SystemHealthDto to SOAP model.
    /// </summary>
    public static SystemHealthSoap ToSoap(this SystemHealthDto dto)
    {
        return new SystemHealthSoap
        {
            TotalProviders = dto.TotalProviders,
            ActiveProviders = dto.ActiveProviders,
            QuarantinedProviders = dto.QuarantinedProviders,
            TotalCurrencies = dto.TotalCurrencies,
            TotalExchangeRates = dto.TotalExchangeRates,
            LatestRateDate = dto.LatestRateDate?.ToString("yyyy-MM-dd"),
            OldestRateDate = dto.OldestRateDate?.ToString("yyyy-MM-dd"),
            TotalFetchesToday = dto.TotalFetchesToday,
            SuccessfulFetchesToday = dto.SuccessfulFetchesToday,
            FailedFetchesToday = dto.FailedFetchesToday,
            SuccessRateToday = dto.SuccessRateToday,
            LastUpdated = dto.LastUpdated
        };
    }

    /// <summary>
    /// Converts an ErrorSummaryDto to SOAP model.
    /// </summary>
    public static ErrorSummarySoap ToSoap(this ErrorSummaryDto dto)
    {
        return new ErrorSummarySoap
        {
            ErrorType = dto.ErrorType,
            ErrorMessage = dto.ErrorMessage,
            OccurrenceCount = dto.OccurrenceCount,
            FirstOccurrence = dto.FirstOccurrence,
            LastOccurrence = dto.LastOccurrence,
            AffectedProviders = dto.AffectedProviders.ToArray()
        };
    }

    /// <summary>
    /// Converts a collection of ErrorSummaryDto to SOAP models.
    /// </summary>
    public static ErrorSummarySoap[] ToSoap(this IEnumerable<ErrorSummaryDto> dtos)
    {
        return dtos.Select(dto => dto.ToSoap()).ToArray();
    }

    /// <summary>
    /// Converts a FetchActivityDto to SOAP model.
    /// </summary>
    public static FetchActivitySoap ToSoap(this FetchActivityDto dto)
    {
        return new FetchActivitySoap
        {
            LogId = dto.LogId,
            ProviderId = dto.ProviderId,
            ProviderCode = dto.ProviderCode,
            ProviderName = dto.ProviderName,
            Status = dto.Status,
            RatesImported = dto.RatesImported,
            RatesUpdated = dto.RatesUpdated,
            ErrorMessage = dto.ErrorMessage,
            StartedAt = dto.StartedAt,
            CompletedAt = dto.CompletedAt,
            Duration = dto.Duration?.ToString(@"hh\:mm\:ss")
        };
    }

    /// <summary>
    /// Converts a collection of FetchActivityDto to SOAP models.
    /// </summary>
    public static FetchActivitySoap[] ToSoap(this IEnumerable<FetchActivityDto> dtos)
    {
        return dtos.Select(dto => dto.ToSoap()).ToArray();
    }
}
