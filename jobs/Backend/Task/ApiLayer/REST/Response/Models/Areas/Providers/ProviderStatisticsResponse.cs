namespace REST.Response.Models.Areas.Providers;

/// <summary>
/// API response model for provider statistics and performance metrics.
/// </summary>
public class ProviderStatisticsResponse
{
    /// <summary>
    /// Provider unique identifier.
    /// </summary>
    public int ProviderId { get; set; }

    /// <summary>
    /// Provider code.
    /// </summary>
    public string ProviderCode { get; set; } = string.Empty;

    /// <summary>
    /// Provider name.
    /// </summary>
    public string ProviderName { get; set; } = string.Empty;

    /// <summary>
    /// Total number of exchange rates provided.
    /// </summary>
    public int TotalRatesProvided { get; set; }

    /// <summary>
    /// Total number of fetch attempts.
    /// </summary>
    public int TotalFetchAttempts { get; set; }

    /// <summary>
    /// Number of successful fetches.
    /// </summary>
    public int SuccessfulFetches { get; set; }

    /// <summary>
    /// Number of failed fetches.
    /// </summary>
    public int FailedFetches { get; set; }

    /// <summary>
    /// Success rate percentage.
    /// </summary>
    public decimal SuccessRate { get; set; }

    /// <summary>
    /// Date of first fetch.
    /// </summary>
    public DateTimeOffset? FirstFetchDate { get; set; }

    /// <summary>
    /// Date of last fetch.
    /// </summary>
    public DateTimeOffset? LastFetchDate { get; set; }

    /// <summary>
    /// Average interval between fetches (formatted).
    /// </summary>
    public string? AverageFetchInterval { get; set; }

    /// <summary>
    /// Oldest exchange rate date provided.
    /// </summary>
    public string? OldestRateDate { get; set; }

    /// <summary>
    /// Newest exchange rate date provided.
    /// </summary>
    public string? NewestRateDate { get; set; }

    /// <summary>
    /// How many days of data this provider covers.
    /// </summary>
    public int? DataCoverageDays
    {
        get
        {
            if (OldestRateDate != null && NewestRateDate != null)
            {
                var oldest = DateOnly.Parse(OldestRateDate);
                var newest = DateOnly.Parse(NewestRateDate);
                return newest.DayNumber - oldest.DayNumber;
            }
            return null;
        }
    }
}
