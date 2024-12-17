namespace ExchangeRateUpdater.Infrastructure.HttpClients.Config;

public class ExchangeRatesSettings
{
    public const string SectionName = "ExchangeRates";
    public string CnbApi { get; set; } = null!;
}