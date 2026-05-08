using System;

namespace ExchangeRateUpdater.Models.Cache;

public class CacheObject<T>
{
    public T Data { get; set; }
    public DateTimeOffset DataExtractionTimeUTC { get; set; }
}
