using System.ComponentModel.DataAnnotations;

namespace ExchangeRateUpdater.Infrastructure;

public sealed class CnbOptions
{
    public const string SectionName = "Cnb";

    public string Url { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int TimeoutSeconds { get; set; } = 10;

    [Range(0, int.MaxValue)]
    public int MaxRetries { get; set; } = 3;

    [Range(1, int.MaxValue)]
    public int CacheTtlSeconds { get; set; } = 3600;
}
