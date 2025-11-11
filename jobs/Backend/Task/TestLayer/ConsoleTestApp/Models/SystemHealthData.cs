namespace ConsoleTestApp.Models;

public class SystemHealthData
{
    public string Status { get; set; } = string.Empty;
    public int TotalProviders { get; set; }
    public int HealthyProviders { get; set; }
    public int UnhealthyProviders { get; set; }
    public int TotalCurrencies { get; set; }
    public int TotalUsers { get; set; }
    public DateTime? LastFetchTime { get; set; }
    public long TotalExchangeRates { get; set; }
    public double SystemUptime { get; set; }
}

public class ErrorSummaryData
{
    public int Id { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string? Severity { get; set; }
    public string? SourceComponent { get; set; }
    public DateTime OccurredAt { get; set; }
    public int? ProviderId { get; set; }
    public string? ProviderCode { get; set; }
}

public class ErrorsListData
{
    public List<ErrorSummaryData> Errors { get; set; } = new();
    public int TotalCount { get; set; }
}

public class FetchActivityData
{
    public int Id { get; set; }
    public int ProviderId { get; set; }
    public string ProviderCode { get; set; } = string.Empty;
    public string ProviderName { get; set; } = string.Empty;
    public DateTime FetchedAt { get; set; }
    public bool Success { get; set; }
    public int? RatesCount { get; set; }
    public string? ErrorMessage { get; set; }
    public long DurationMs { get; set; }
}

public class FetchActivityListData
{
    public List<FetchActivityData> Activities { get; set; } = new();
    public int TotalCount { get; set; }
}
