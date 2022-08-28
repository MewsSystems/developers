namespace ExchangeRateUpdater.Models;

public class AppConfig
{
    public const string SectionKey = nameof(AppConfig);

    public string Uri { get; set; }
    public string BaseCurrencyCode { get; set; }
    public string[] CurrencyCodesFilter { get; set; }
}