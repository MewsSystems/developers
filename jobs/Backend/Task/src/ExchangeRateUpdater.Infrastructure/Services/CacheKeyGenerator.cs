namespace ExchangeRateUpdater.Infrastructure.Services;

public static class CacheKeyGenerator
{
    private const string ProviderPrefix = "ExchangeRates";
    
    public static string GenerateDailyRatesKey(string providerName, DateTime? date = null)
    {
        var dateSuffix = date.HasValue ? $"_{date.Value:yyyy-MM-dd}" : "";
        return $"{ProviderPrefix}_{providerName}_DailyRates{dateSuffix}";
    }
    
    public static string GenerateMonthlyRatesKey(string providerName, DateTime? date = null)
    {
        var dateSuffix = date.HasValue ? $"_{date.Value:yyyy-MM}" : "";
        return $"{ProviderPrefix}_{providerName}_MonthlyRates{dateSuffix}";
    }
    
    public static string GenerateCustomKey(string providerName, string suffix)
    {
        return $"{ProviderPrefix}_{providerName}_{suffix}";
    }
} 