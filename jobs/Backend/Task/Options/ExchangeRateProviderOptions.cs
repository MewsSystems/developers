namespace ExchangeRateUpdater.Options;

public class ExchangeRateProviderOptions
{
    public const string Key = "ExchangeRateProviderUrl";
    
    public string BaseUrl { get; init; } = string.Empty;
    public string OtherCurrenciesUrl { get; init; } = string.Empty;
}