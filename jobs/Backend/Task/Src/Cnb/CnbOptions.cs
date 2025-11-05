namespace ExchangeRateUpdater.Src.Cnb;

public class CnbOptions
{
    public bool EnablePublishTimeFallback { get; set; } = true;
    public string PublishTimeZone { get; set; } = "Europe/Prague";
    public TimeOnly PublishTime { get; set; } = new(14, 30);

    public TimeSpan HttpTimeout { get; set; } = TimeSpan.FromSeconds(15);
    public int RetryCount { get; set; } = 3;

    public TimeSpan CacheTtl { get; set; } = TimeSpan.FromDays(30);
    public string CacheKeyPrefix { get; set; } = "cnb:rates:";

    public string JsonApiBase { get; set; } = "https://api.cnb.cz/cnbapi";
    public string JsonDailyEndpoint { get; set; } = "/exrates/daily";
    public string JsonDateParam { get; set; } = "date";
    public string JsonDateFormat { get; set; } = "yyyy-MM-dd";
}
