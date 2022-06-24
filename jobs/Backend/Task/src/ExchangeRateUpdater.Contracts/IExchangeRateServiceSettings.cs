namespace ExchangeRateUpdater.Contracts;

public interface IExchangeRateServiceSettings
{
    /// <summary> Url that exchange rates will be pulled from </summary>
    string BaseUrl { get; }

    /// <summary> Timezone id where exchange rate data belongs </summary>
    string TimezoneId { get; }

    /// <summary> Target currency </summary>
    string DefaultCurrency { get; }

    /// <summary> Delimiter to separate the lines of exchange rate response </summary>
    string MappingDelimiter { get; }

    string MappingDecimalSeparator { get; }

    /// <summary> If false, mapping exceptions will be ignored and not added to response of exchange rates </summary>
    bool ThrowExceptionOnMappingErrors { get; }

    /// <summary> Determines if in memory cache will be used </summary>
    bool UseInMemoryCache { get; }

    /// <summary>
    /// Time to be added to current day for cache expiration
    /// If current time is already bigger than the expiry time of day, then expiration will be calculated for next day
    /// </summary>
    TimeSpan CacheExpiryTime { get; }
}