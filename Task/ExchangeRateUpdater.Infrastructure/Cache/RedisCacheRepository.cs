namespace ExchangeRateUpdater.Infrastructure.Cache;

using System;
using System.Text.Json;
using Application.ExchangeRates.Dtos;
using Domain.Repositories;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using Application.Common.Mappings;
using Domain.Common;
using Domain.Entities;

public class RedisCacheRepository : ICacheRepository
{
    private readonly IDatabase _database;
    private readonly ILogger<RedisCacheRepository> _logger;

    public RedisCacheRepository(IConnectionMultiplexer connection, ILogger<RedisCacheRepository> logger)
    {
        Ensure.Argument.NotNull(connection, nameof(connection));
        Ensure.Argument.NotNull(logger, nameof(logger));
        _database = connection.GetDatabase();
        _logger = logger;
    }

    public async Task<Dictionary<string, ExchangeRate>?> GetRatesDictionaryAsync(string key)
    {
        try
        {
            var entries = await _database.HashGetAllAsync(key);
            if (entries.Length == 0)
            {
                _logger.LogInformation("{Key} not found in redis cache", key);
                return default;
            }

            var dict = new Dictionary<string, ExchangeRate>();
            foreach (var entry in entries)
            {
                var dto = JsonSerializer.Deserialize<ExchangeRateDto>(entry.Value!);
                if (dto != null)
                {
                    dict[entry.Name!] = dto.ToEntity();
                }
            }

            _logger.LogInformation("{Key} found in redis cache (hash)", key);
            return dict;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to get or deserialize cached value for {Key}", key);
            throw;
        }
    }

    public async Task<List<ExchangeRate>?> GetRatesListAsync(string key)
    {
        try
        {
            var dictionary = await GetRatesDictionaryAsync(key);
            return dictionary?.Values.ToList();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to get or deserialize cached value for {Key}", key);
            throw;
        }
    }

    public async Task SetRatesAsync(string key, Dictionary<string, ExchangeRate> value)
    {
        try
        {
            var hashEntries = value.Select(pair =>
                new HashEntry(pair.Key, JsonSerializer.Serialize(pair.Value.ToDto()))
            ).ToArray();

            await _database.HashSetAsync(key, hashEntries);
            _logger.LogInformation("{Key} added to redis cache (hash)", key);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to set cache for {Key}", key);
        }
    }

    public async Task SetRatesAsync(string key, Dictionary<string, ExchangeRate> value, TimeSpan expirationDate)
    {
        try
        {
            var hashEntries = value.Select(pair =>
                new HashEntry(pair.Key, JsonSerializer.Serialize(pair.Value.ToDto()))
            ).ToArray();

            await _database.HashSetAsync(key, hashEntries);
            await _database.KeyExpireAsync(key, expirationDate);
            _logger.LogInformation("{Key} added to redis cache (hash)", key);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to set cache for {Key}", key);
            throw;
        }
    }

    public async Task SetRatesListAsync(string key, List<ExchangeRate> value, TimeSpan expirationDate)
    {
        try
        {
            var json = JsonSerializer.Serialize(value.Select(r => r.ToDto()));
            await _database.StringSetAsync(key, json, expirationDate);
            _logger.LogInformation("{Key} added to redis cache (string)", key);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to set cache for {Key}", key);
            throw;
        }
    }

    public async Task SetRatesListAsync(string key, List<ExchangeRate> value)
    {
        var defaultExpiration = TimeSpan.FromDays(365);
        await SetRatesListAsync(key, value, defaultExpiration);
    }

    public async Task ClearCacheAsync(string key)
    {
        try
        {
            await _database.KeyDeleteAsync(key);
            _logger.LogInformation("{Key} removed from redis cache", key);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to clear cache for {Key}", key);
        }
    }
}