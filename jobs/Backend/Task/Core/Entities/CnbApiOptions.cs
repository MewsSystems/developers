namespace ExchangeRateUpdater.Core.Entities;

public class CnbApiOptions
{
    public string BaseUrl { get; set; } = default!;
    public string DailyRatesEndpoint { get; set; } = default!;
}