namespace ExchangeRateUpdater.Configuration;

public sealed record ApiConfiguration
{
    public string Name { get; init; } = string.Empty;
    public string BaseUrl { get; init; } = string.Empty;
    public string ExchangeRateEndpoint { get; init; } = string.Empty;
    public string Language { get; init; } = "EN";
    public int RequestTimeoutInSeconds { get; init; } = 20;
    public int RetryTimeOutInSeconds { get; init; } = 5;
    public Dictionary<string, string> DefaultRequestHeaders { get; init; } = [];
}