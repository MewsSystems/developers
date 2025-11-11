namespace DataLayer.DTOs;

// vw_CurrentExchangeRates
public class CurrentExchangeRateView
{
    public int Id { get; set; }
    public string ProviderCode { get; set; } = string.Empty;
    public string ProviderName { get; set; } = string.Empty;
    public string BaseCurrencyCode { get; set; } = string.Empty;
    public string TargetCurrencyCode { get; set; } = string.Empty;
    public int TargetCurrencyId { get; set; }
    public decimal Rate { get; set; }
    public int Multiplier { get; set; }
    public decimal RatePerUnit { get; set; }
    public DateOnly ValidDate { get; set; }
    public DateTimeOffset Created { get; set; }
}

// vw_LatestExchangeRates
public class LatestExchangeRateView
{
    public int Id { get; set; }
    public string ProviderCode { get; set; } = string.Empty;
    public string ProviderName { get; set; } = string.Empty;
    public string BaseCurrencyCode { get; set; } = string.Empty;
    public string TargetCurrencyCode { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public int Multiplier { get; set; }
    public decimal RatePerUnit { get; set; }
    public DateOnly ValidDate { get; set; }
    public DateTimeOffset Created { get; set; }
    public int RowNum { get; set; }
}

// vw_ProviderHealthStatus
public class ProviderHealthStatusView
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int BaseCurrencyId { get; set; }
    public string BaseCurrencyCode { get; set; } = string.Empty;
    public bool RequiresAuthentication { get; set; }
    public DateTimeOffset? LastSuccessfulFetch { get; set; }
    public DateTimeOffset? LastFailedFetch { get; set; }
    public int ConsecutiveFailures { get; set; }
    public int? HoursSinceLastSuccess { get; set; }
    public int TotalFetches30Days { get; set; }
    public int SuccessfulFetches30Days { get; set; }
    public int FailedFetches30Days { get; set; }
    public long? AvgFetchDurationMs { get; set; }
    public string HealthStatus { get; set; } = string.Empty;
}

// vw_ExchangeRateHistory
public class ExchangeRateHistoryView
{
    public int Id { get; set; }
    public string ProviderCode { get; set; } = string.Empty;
    public string BaseCurrencyCode { get; set; } = string.Empty;
    public string TargetCurrencyCode { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public int Multiplier { get; set; }
    public decimal RatePerUnit { get; set; }
    public DateOnly ValidDate { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Modified { get; set; }
    public int DaysOld { get; set; }
}

// vw_RecentFetchActivity
public class RecentFetchActivityView
{
    public long Id { get; set; }
    public int ProviderId { get; set; }
    public string ProviderCode { get; set; } = string.Empty;
    public string ProviderName { get; set; } = string.Empty;
    public DateTimeOffset FetchStarted { get; set; }
    public DateTimeOffset? FetchCompleted { get; set; }
    public string Status { get; set; } = string.Empty;
    public int? RatesImported { get; set; }
    public int? RatesUpdated { get; set; }
    public int? DurationMs { get; set; }
    public string? ErrorMessage { get; set; }
    public string? RequestedByEmail { get; set; }
}

// vw_ErrorSummary
public class ErrorSummaryView
{
    public long Id { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public string Severity { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? UserEmail { get; set; }
    public int MinutesAgo { get; set; }
}

// vw_CurrencyPairAvailability
public class CurrencyPairAvailabilityView
{
    public string BaseCurrencyCode { get; set; } = string.Empty;
    public string TargetCurrencyCode { get; set; } = string.Empty;
    public int ProviderCount { get; set; }
    public string AvailableProviders { get; set; } = string.Empty;
    public DateOnly? LatestDate { get; set; }
    public int? DaysSinceUpdate { get; set; }
}

// vw_SystemHealthDashboard
public class SystemHealthDashboardView
{
    public string Metric { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Details { get; set; }
}

// vw_AllLatestExchangeRates
public class AllLatestExchangeRatesView
{
    public int Id { get; set; }
    public int ProviderId { get; set; }
    public string ProviderCode { get; set; } = string.Empty;
    public string ProviderName { get; set; } = string.Empty;
    public string BaseCurrencyCode { get; set; } = string.Empty;
    public int BaseCurrencyId { get; set; }
    public string TargetCurrencyCode { get; set; } = string.Empty;
    public int TargetCurrencyId { get; set; }
    public decimal Rate { get; set; }
    public int Multiplier { get; set; }
    public decimal RatePerUnit { get; set; }
    public DateOnly ValidDate { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Modified { get; set; }
    public int DaysOld { get; set; }
    public string FreshnessStatus { get; set; } = string.Empty;
}

// vw_AllLatestUpdatedExchangeRates
public class AllLatestUpdatedExchangeRatesView
{
    public int Id { get; set; }
    public int ProviderId { get; set; }
    public string ProviderCode { get; set; } = string.Empty;
    public string ProviderName { get; set; } = string.Empty;
    public string BaseCurrencyCode { get; set; } = string.Empty;
    public int BaseCurrencyId { get; set; }
    public string TargetCurrencyCode { get; set; } = string.Empty;
    public int TargetCurrencyId { get; set; }
    public decimal Rate { get; set; }
    public int Multiplier { get; set; }
    public decimal RatePerUnit { get; set; }
    public DateOnly ValidDate { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Modified { get; set; }
    public int DaysOld { get; set; }
    public int MinutesSinceUpdate { get; set; }
    public string UpdateFreshness { get; set; } = string.Empty;
}
