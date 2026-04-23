using System.ComponentModel.DataAnnotations;

namespace ExchangeRateUpdater.Api.Infrastructure.Providers.Cnb;

public sealed class CnbApiOptions
{
    public const string SectionName = "CnbApi";

    [Required]
    public required string BaseUrl { get; set; }

    public string Language { get; set; } = "EN";

    public int CacheDurationMinutes { get; set; } = 60;

    public int HistoricalCacheDurationMinutes { get; set; } = 1440;
}
