namespace ExchangeRateUpdater.Infrastructure.Cache;

using System;
using System.Text.Json;
using Domain.Repositories;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

public class RedisCacheRepository : ICacheRepository
{
    private readonly IConnectionMultiplexer connection;
    private readonly IDatabase database;
    private readonly ILogger<RedisCacheRepository> logger;

    public RedisCacheRepository(IConnectionMultiplexer connection, ILogger<RedisCacheRepository> logger)
    {
        this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.database = this.connection.GetDatabase();
    }

    public T GetFromCache<T>(string key)
    {
        var value = database.StringGet(key);
        if (!value.HasValue)
        {
            logger.LogInformation("{Key} not found in redis cache", key);
            return default!;
        }

        try
        {
            var result = JsonSerializer.Deserialize<T>(value!);
            logger.LogInformation("{Key} found in redis cache", key);
            return result!;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to deserialize cached value for {Key}", key);
            return default!;
        }
    }

    public void SetCache<T>(string key, T value, TimeSpan expirationDate)
    {
        var json = JsonSerializer.Serialize(value);
        database.StringSet(key, json, expirationDate);
        logger.LogInformation("{Key} added to redis cache", key);
    }

    public void SetCache<T>(string key, T value)
    {
        // default to 1 hour
        SetCache(key, value, TimeSpan.FromHours(1));
    }

    public void ClearCache(string key)
    {
        database.KeyDelete(key);
        logger.LogInformation("{Key} removed from redis cache", key);
    }
}
