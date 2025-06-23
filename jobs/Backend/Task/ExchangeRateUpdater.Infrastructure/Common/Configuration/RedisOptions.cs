namespace ExchangeRateUpdater.Infrastructure.Common.Configuration;

public class RedisOptions
{
    public string Url { get; init; }
    public int Port { get; init; }
    public string Password { get; init; }
}