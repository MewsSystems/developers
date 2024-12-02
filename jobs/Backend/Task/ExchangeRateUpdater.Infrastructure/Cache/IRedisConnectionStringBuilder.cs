namespace ExchangeRateUpdater.Infrastructure.Cache;

public interface IRedisConnectionStringBuilder
{
    string Build();
}