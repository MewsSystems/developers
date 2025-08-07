using ExchangeRateUpdater.Infrastructure.Configuration;

namespace ExchangeRateUpdater.Infrastructure.Configuration;

public class ExchangeRateProvidersConfig
{
    public CzechNationalBankExchangeRateConfig? CzechNationalBank { get; init; }
    
    // IConfiguration-style access with colon separators
    public CacheConfig? this[string key]
    {
        get
        {
            var parts = key.Split(':', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2 || parts[1] != "Cache")
                return null;
                
            return parts[0] switch
            {
                "CzechNationalBank" => CzechNationalBank?.Cache,
                _ => null
            };
        }
    }
}

public class CzechNationalBankExchangeRateConfig
{
    public string BaseUrl { get; init; } = "https://api.cnb.cz/cnbapi/";
    public int TimeoutSeconds { get; init; } = 30;
    public CacheConfig Cache { get; init; } = new();
} 