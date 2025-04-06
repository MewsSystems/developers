namespace ExchangeRateUpdater.Options;

public class ExchangeRateProviderOptions
{
    public const string Key = "ExchangeRateProviderUrl";
    
    public string BaseUrl { get; set; } = string.Empty;
    public string OtherCurrenciesUrl { get; set; } = string.Empty;
}