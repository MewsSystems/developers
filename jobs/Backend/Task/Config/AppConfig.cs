using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExchangeRateUpdater.Config;

public sealed class AppConfig
{
    public ExchangeRateSettings ExchangeRateSettings { get; init; } = new();
}

public sealed class ExchangeRateSettings
{
    [Required] public Uri CnbDailyUrl { get; init; } = default!;
    [Range(1, 120)] public int HttpTimeoutSeconds { get; init; } = 10;
    public List<string> Currencies { get; init; } = new();
}
