namespace ExchangeRateUpdater.Constants;

/// <summary>
/// Contains all log message templates used throughout the application.
/// </summary>
public static class LogMessages
{
    public static class CnbApiClient
    {
        public const string FetchingExchangeRates = "Fetching exchange rates from CNB API: {ApiUrl}";
        public const string FetchSuccessful = "Successfully fetched exchange rates from CNB API";
        public const string HttpRequestFailed = "HTTP request failed while fetching exchange rates from CNB";
        public const string RequestTimedOut = "Request to CNB API timed out";
        public const string UnexpectedError = "Unexpected error while fetching exchange rates from CNB";
    }

    public static class CnbDataParser
    {
        public const string EmptyOrNullData = "Received empty or null data to parse";
        public const string InsufficientLines = "Data contains fewer than expected lines (header + column names + data)";
        public const string FailedToParseLine = "Failed to parse line: {Line}";
        public const string ParseSuccessful = "Successfully parsed {Count} exchange rates";
        public const string UnexpectedColumnCount = "Line has unexpected number of columns. Expected: {Expected}, Actual: {Actual}, Line: {Line}";
        public const string FailedToParseAmount = "Failed to parse amount: {Amount}";
        public const string FailedToParseRate = "Failed to parse rate: {Rate}";
    }

    public static class ExchangeRateCache
    {
        public const string CacheHit = "Cache hit for key: {CacheKey}, {Count} rates retrieved";
        public const string CacheMiss = "Cache miss for key: {CacheKey}";
        public const string AttemptedCacheEmpty = "Attempted to cache empty rate list";
        public const string CachedRates = "Cached {Count} exchange rates for key: {CacheKey}, TTL: {TTL} minutes";
        public const string ClearedEntries = "Cleared {Count} cache entries";
    }

    public static class SupportedCurrenciesCache
    {
        public const string CacheHit = "Cache hit for supported currencies: {Count} currencies";
        public const string CacheMiss = "Cache miss for supported currencies";
        public const string AttemptedCacheEmpty = "Attempted to cache empty currency list";
        public const string CachedCurrencies = "Cached {Count} supported currencies, TTL: {TTL} minutes";
        public const string ClearedCache = "Cleared supported currencies cache";
    }

    public static class ExchangeRateProvider
    {
        public const string NoCurrenciesRequested = "No currencies requested";
        public const string FetchingExchangeRates = "Fetching exchange rates for {Count} currencies";
        public const string ReturningFromCache = "Returning {Count} exchange rates from cache";
        public const string RetrievalSuccessful = "Successfully retrieved {Retrieved} exchange rates out of {Requested} requested currencies";
        public const string UnexpectedError = "Unexpected error while getting exchange rates";
        public const string ReturningCachedSupportedCurrencies = "Returning supported currencies from cache";
        public const string FetchingSupportedCurrencies = "Fetching supported currencies from CNB";
        public const string FoundSupportedCurrencies = "Found {Count} supported currencies";
        public const string FailedToFetchSupportedCurrencies = "Failed to fetch supported currencies from CNB";
    }
}
