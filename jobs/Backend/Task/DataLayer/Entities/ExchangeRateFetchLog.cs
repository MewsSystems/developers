namespace DataLayer.Entities;

public class ExchangeRateFetchLog
{
    public long Id { get; set; }
    public int ProviderId { get; set; }
    public DateTimeOffset FetchStarted { get; set; }
    public DateTimeOffset? FetchCompleted { get; set; }
    public string Status { get; set; } = "Running";
    public int? RatesImported { get; set; }
    public int? RatesUpdated { get; set; }
    public string? ErrorMessage { get; set; }
    public int? RequestedBy { get; set; }
    public int? DurationMs { get; set; }

    // Navigation properties
    public ExchangeRateProvider Provider { get; set; } = null!;
    public User? RequestedByUser { get; set; }
}
