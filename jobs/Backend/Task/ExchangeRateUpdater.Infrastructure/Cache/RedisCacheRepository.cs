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
using Domain.Entities;

public class RedisCacheRepository : ICacheRepository
{
    private readonly IDatabase _database;
    private readonly ILogger<RedisCacheRepository> _logger;

    public RedisCacheRepository(IConnectionMultiplexer connection, ILogger<RedisCacheRepository> logger)
    {
        _database = connection?.GetDatabase() ?? throw new ArgumentNullException(nameof(connection));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
            return default;
        }
    }

    public async Task<List<ExchangeRate>?> GetRatesListAsync(string key)
    {
        try
        {
            var value = await _database.StringGetAsync(key).ConfigureAwait(false);
            if (!value.HasValue)
            {
                _logger.LogInformation("{Key} not found in redis cache", key);
                return default;
            }

            var dtos = JsonSerializer.Deserialize<List<ExchangeRateDto>>(value!);
            if (dtos == null) return default;
            var list = dtos.Select(d => d.ToEntity()).ToList();
            _logger.LogInformation("{Key} found in redis cache (string)", key);
            return list;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to get or deserialize cached value for {Key}", key);
            return default;
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