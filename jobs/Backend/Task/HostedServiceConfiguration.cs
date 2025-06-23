namespace ExchangeRateUpdater;

internal sealed class HostedServiceConfiguration
{
    [JsonPropertyName("TargetCurrencies")]
    public string[] TargetCurrencies { get; init; } = Array.Empty<string>();
}