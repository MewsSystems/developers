using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Interfaces;
using System.Text.Json;

namespace ExchangeRateUpdater.Services.Cache;

public class FileCacheService : ICacheService
{
    private readonly string _filePath;
    private readonly object _lock = new();
    private readonly Dictionary<string, CacheEntry> _cache;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    public FileCacheService(string filePath)
    {
        _filePath = filePath;
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            _cache = JsonSerializer.Deserialize<Dictionary<string, CacheEntry>>(json, _jsonOptions)
                     ?? new Dictionary<string, CacheEntry>();
        }
        else
        {
            _cache = new Dictionary<string, CacheEntry>();
        }

        CleanupExpiredEntries();
    }

    public Task<T> GetAsync<T>(string key)
    {
        lock (_lock)
        {
            if (_cache.TryGetValue(key, out var entry))
            {
                if (entry.ExpiresAt == null || entry.ExpiresAt > DateTime.UtcNow)
                {
                    return Task.FromResult(JsonSerializer.Deserialize<T>(entry.JsonValue, _jsonOptions));
                }
                else
                {
                    _cache.Remove(key);
                    Save();
                }
            }
        }

        return Task.FromResult<T>(default);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan ttl)
    {
        var entry = new CacheEntry
        {
            JsonValue = JsonSerializer.Serialize(value, _jsonOptions),
            ExpiresAt = DateTime.UtcNow.Add(ttl)
        };

        lock (_lock)
        {
            _cache[key] = entry;
            Save();
        }

        return Task.CompletedTask;
    }

    private void Save()
    {
        var json = JsonSerializer.Serialize(_cache, _jsonOptions);
        File.WriteAllText(_filePath, json);
    }

    private void CleanupExpiredEntries()
    {
        var expiredKeys = _cache
            .Where(kv => kv.Value.ExpiresAt != null && kv.Value.ExpiresAt <= DateTime.UtcNow)
            .Select(kv => kv.Key)
            .ToList();

        foreach (var key in expiredKeys)
            _cache.Remove(key);

        if (expiredKeys.Any())
            Save();
    }

    private class CacheEntry
    {
        public string JsonValue { get; set; } = string.Empty;
        public DateTime? ExpiresAt { get; set; }
    }
}
