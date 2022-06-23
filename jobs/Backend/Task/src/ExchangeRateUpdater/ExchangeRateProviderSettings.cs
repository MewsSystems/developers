using ExchangeRateUpdater.Contracts;

namespace ExchangeRateUpdater;

public class ExchangeRateProviderSettings : IExchangeRateServiceSettings
{
    public ExchangeRateProviderSettings (string baseUrl,
                                         string timezoneId,
                                         string defaultCurrency,
                                         string mappingDelimiter,
                                         string mappingDecimalSeparator,
                                         bool throwExceptionOnMappingErrors,
                                         bool useInMemoryCache,
                                         TimeSpan cacheExpiryTime)
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new ArgumentNullException(nameof(baseUrl), "BaseUrl should not be null, empty or whitespace.");
        if (string.IsNullOrWhiteSpace(timezoneId))
            throw new ArgumentNullException(nameof(timezoneId), "TimezoneId should not be null, empty or whitespace.");
        if (string.IsNullOrWhiteSpace(defaultCurrency))
            throw new ArgumentNullException(nameof(defaultCurrency), "DefaultCurrency should not be null, empty or whitespace.");
        if (string.IsNullOrWhiteSpace(mappingDelimiter))
            throw new ArgumentNullException(nameof(mappingDelimiter), "MappingDelimiter should not be null, empty or whitespace.");
        if (string.IsNullOrWhiteSpace(mappingDecimalSeparator))
            throw new ArgumentNullException(nameof(mappingDecimalSeparator), "MappingDecimalSeparator should not be null, empty or whitespace.");

        BaseUrl                       = baseUrl;
        TimezoneId                    = timezoneId;
        DefaultCurrency               = defaultCurrency;
        MappingDelimiter              = mappingDelimiter;
        MappingDecimalSeparator       = mappingDecimalSeparator;
        ThrowExceptionOnMappingErrors = throwExceptionOnMappingErrors;
        UseInMemoryCache              = useInMemoryCache;
        CacheExpiryTime               = cacheExpiryTime;
    }
    
    public string BaseUrl { get; }
    public string TimezoneId { get; }
    public string DefaultCurrency { get; }
    public string MappingDelimiter { get; }
    public string MappingDecimalSeparator { get; }
    public bool ThrowExceptionOnMappingErrors { get; }
    public bool UseInMemoryCache { get; }
    public TimeSpan CacheExpiryTime { get; }
}