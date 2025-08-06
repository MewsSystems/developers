namespace ExchangeRateUpdater.Infrastructure.Services;

public static class CacheKeyGenerator
{
    private const string ProviderPrefix = "ExchangeRates";
    
    public static string GenerateDailyRatesKey(string providerName, DateTime? date = null)
    {
        if (string.IsNullOrWhiteSpace(providerName))
        {
            throw new ArgumentException("Provider name cannot be null, empty, or whitespace", nameof(providerName));
        }

        var dateSuffix = date.HasValue ? $":{date.Value:yyyy-MM-dd}" : "";
        return $"{ProviderPrefix}:{providerName}:Daily{dateSuffix}";
    }
    
    public static string GenerateMonthlyRatesKey(string providerName, DateTime? date = null)
    {
        if (string.IsNullOrWhiteSpace(providerName))
        {
            throw new ArgumentException("Provider name cannot be null, empty, or whitespace", nameof(providerName));
        }

        var dateSuffix = date.HasValue ? $":{date.Value:yyyy-MM}" : "";
        return $"{ProviderPrefix}:{providerName}:Monthly{dateSuffix}";
    }
    
    public static string GenerateCustomKey(string providerName, string suffix)
    {
        if (string.IsNullOrWhiteSpace(providerName))
        {
            throw new ArgumentException("Provider name cannot be null, empty, or whitespace", nameof(providerName));
        }

        if (string.IsNullOrWhiteSpace(suffix))
        {
            throw new ArgumentException("Suffix cannot be null, empty, or whitespace", nameof(suffix));
        }

        return $"{ProviderPrefix}:{providerName}:{suffix}";
    }
} 