namespace ExchangeRateUpdater.ConfigurationOptions;

public class CnbClientOptions
{
    public const string CnbClient = "CnbClient";
    
    public required string BaseUri { get; init; }
    public required string DailyExchangeRatesPath { get; init; }
    public int CacheLifeTimeInSeconds { get; init; }
}