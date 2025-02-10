namespace ExchangeRateUpdater.Application.Settings;

/// <summary>
/// Represents cache configuration settings.
/// </summary>
public class CacheSettings
{
    /// <summary>
    /// Gets or sets the cache duration in minutes.
    /// </summary>
    /// <value>
    /// The cache duration as an integer. Defaults to <c>30</c> minutes.
    /// </value>
    public int Duration { get; set; } = 30;
}
