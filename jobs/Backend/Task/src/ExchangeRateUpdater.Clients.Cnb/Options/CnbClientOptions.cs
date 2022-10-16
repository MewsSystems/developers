namespace ExchangeRateUpdater.Clients.Cnb.Options;

/// <summary>
/// Cnb client options.
/// </summary>
public sealed class CnbClientOptions
{
    /// <summary>
    /// The base URL for the Cnb API.
    /// </summary>
    public Uri? BaseUrl { get; set; }

    /// <summary>
    /// Gets or sets the retry options.
    /// </summary>
    public RetryOptions RetryOptions { get; set; } = new();
}