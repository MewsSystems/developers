namespace ExchangeRatesUpdater.Common;

public class AppConfiguration
{
    public string[] DefaultCurrencies { get; set; } = Array.Empty<string>();
    public string DefaultBank { get; set; } = string.Empty;
    public Dictionary<string, string> ProviderAPIs { get; set; } = new();
}
