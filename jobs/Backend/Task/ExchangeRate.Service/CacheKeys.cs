namespace ExchangeRate.Service;

public static class CacheKeys
{
    public static string ExchangeRateKey(string keyword)
    {
        return $"ExchangeRate_{keyword}";
    }
}