using ConfigurationLayer.Interface;
using ConfigurationLayer.Option;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ConfigurationLayer.Service;

/// <summary>
/// Service for loading exchange rate provider configurations.
/// Implements database-first pattern with appsettings.json fallback.
/// Priority: Cache → Database → appsettings.json
/// </summary>
public class ProviderConfigurationService : IProviderConfigurationService
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
    private readonly IOptions<ExchangeRateProvidersOptions> _fallbackConfig;
    private readonly IMemoryCache _cache;
    private readonly ILogger<ProviderConfigurationService> _logger;
    private const int CacheExpirationMinutes = 15;
    private const string CacheKeyPrefix = "provider_config_";
    private const string AllProvidersCacheKey = "all_active_providers";
    private const string CacheKeysListKey = "provider_cache_keys";
    private static readonly object _cacheKeysLock = new object();

    public ProviderConfigurationService(
        IDbContextFactory<ApplicationDbContext> contextFactory,
        IOptions<ExchangeRateProvidersOptions> fallbackConfig,
        IMemoryCache cache,
        ILogger<ProviderConfigurationService> logger)
    {
        _contextFactory = contextFactory;
        _fallbackConfig = fallbackConfig;
        _cache = cache;
        _logger = logger;
    }

    /// <summary>
    /// Loads provider configuration by provider code.
    /// Priority: Cache → Database → appsettings.json → null
    /// </summary>
    public async Task<ExchangeRateProviderOptions?> GetProviderConfigurationAsync(string providerCode)
    {
        if (string.IsNullOrWhiteSpace(providerCode))
        {
            _logger.LogWarning("Provider code is null or empty");
            return null;
        }

        var normalizedCode = providerCode.ToUpperInvariant();
        var cacheKey = $"{CacheKeyPrefix}{normalizedCode}";

        // Check cache first
        if (_cache.TryGetValue<ExchangeRateProviderOptions>(cacheKey, out var cachedOptions) && cachedOptions != null)
        {
            _logger.LogDebug("Provider configuration for '{ProviderCode}' retrieved from cache", normalizedCode);
            return cachedOptions;
        }

        try
        {
            // Try database first
            await using var context = await _contextFactory.CreateDbContextAsync();

            var providerEntity = await context.ExchangeRateProviders
                .AsNoTracking()
                .Include(p => p.BaseCurrency)
                .Include(p => p.Configurations)
                .FirstOrDefaultAsync(p => p.Code == normalizedCode);

            if (providerEntity != null)
            {
                _logger.LogDebug("Provider configuration for '{ProviderCode}' retrieved from database", normalizedCode);

                var options = MapEntityToOptions(providerEntity);

                // Cache the value and track the key
                _cache.Set(cacheKey, options, TimeSpan.FromMinutes(CacheExpirationMinutes));
                TrackCacheKey(cacheKey);

                return options;
            }

            _logger.LogDebug("Provider '{ProviderCode}' not found in database, using fallback", normalizedCode);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to read provider configuration for '{ProviderCode}' from database, using fallback", normalizedCode);
        }

        // Fallback to appsettings.json
        var fallbackOptions = GetFallbackProviderOptions(normalizedCode);

        if (fallbackOptions != null)
        {
            _logger.LogDebug("Provider configuration for '{ProviderCode}' retrieved from appsettings.json fallback", normalizedCode);

            // Cache the fallback value and track the key
            _cache.Set(cacheKey, fallbackOptions, TimeSpan.FromMinutes(CacheExpirationMinutes));
            TrackCacheKey(cacheKey);

            return fallbackOptions;
        }

        _logger.LogWarning("Provider '{ProviderCode}' not found in database or appsettings.json", normalizedCode);
        return null;
    }

    /// <summary>
    /// Loads all active provider configurations.
    /// Priority: Cache → Database → appsettings.json
    /// </summary>
    public async Task<List<ExchangeRateProviderOptions>> GetAllActiveProviderConfigurationsAsync()
    {
        // Check cache first
        if (_cache.TryGetValue<List<ExchangeRateProviderOptions>>(AllProvidersCacheKey, out var cachedProviders) && cachedProviders != null)
        {
            _logger.LogDebug("All active provider configurations retrieved from cache");
            return cachedProviders;
        }

        var providers = new List<ExchangeRateProviderOptions>();

        try
        {
            // Try database first
            await using var context = await _contextFactory.CreateDbContextAsync();

            var providerEntities = await context.ExchangeRateProviders
                .AsNoTracking()
                .Include(p => p.BaseCurrency)
                .Include(p => p.Configurations)
                .Where(p => p.IsActive)
                .ToListAsync();

            if (providerEntities.Any())
            {
                _logger.LogDebug("Found {Count} active providers in database", providerEntities.Count);

                providers = providerEntities.Select(MapEntityToOptions).ToList();

                // Cache the value
                _cache.Set(AllProvidersCacheKey, providers, TimeSpan.FromMinutes(CacheExpirationMinutes));

                return providers;
            }

            _logger.LogDebug("No active providers found in database, using fallback");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to read providers from database, using fallback");
        }

        // Fallback to appsettings.json
        var fallbackProviders = GetAllFallbackProviderOptions();

        _logger.LogDebug("Retrieved {Count} providers from appsettings.json fallback", fallbackProviders.Count);

        // Cache the fallback values
        _cache.Set(AllProvidersCacheKey, fallbackProviders, TimeSpan.FromMinutes(CacheExpirationMinutes));

        return fallbackProviders;
    }

    /// <summary>
    /// Refreshes the cached provider configurations.
    /// </summary>
    public Task RefreshCacheAsync()
    {
        _logger.LogInformation("Refreshing provider configuration cache");

        // Remove all provider-related cache entries
        _cache.Remove(AllProvidersCacheKey);

        // Remove individual provider caches using tracked keys
        lock (_cacheKeysLock)
        {
            if (_cache.TryGetValue<HashSet<string>>(CacheKeysListKey, out var cacheKeys) && cacheKeys != null)
            {
                foreach (var key in cacheKeys)
                {
                    _cache.Remove(key);
                }
                _logger.LogDebug("Removed {Count} individual provider cache entries", cacheKeys.Count);

                // Clear the tracking set
                _cache.Remove(CacheKeysListKey);
            }
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Tracks a cache key so it can be cleared during cache refresh.
    /// </summary>
    private void TrackCacheKey(string cacheKey)
    {
        lock (_cacheKeysLock)
        {
            if (!_cache.TryGetValue<HashSet<string>>(CacheKeysListKey, out var cacheKeys))
            {
                cacheKeys = new HashSet<string>();
            }

            cacheKeys!.Add(cacheKey);

            // Store the tracking set with no expiration (managed manually)
            _cache.Set(CacheKeysListKey, cacheKeys, new MemoryCacheEntryOptions
            {
                Priority = CacheItemPriority.NeverRemove
            });
        }
    }

    /// <summary>
    /// Maps DataLayer ExchangeRateProvider entity to ExchangeRateProviderOptions.
    /// </summary>
    private ExchangeRateProviderOptions MapEntityToOptions(DataLayer.Entities.ExchangeRateProvider entity)
    {
        // Validate entity and navigation properties
        if (entity.BaseCurrency == null)
        {
            throw new InvalidOperationException($"Provider '{entity.Code}' has no base currency configured");
        }

        // Build configuration dictionary with duplicate key handling
        var configDict = new Dictionary<string, string>();
        if (entity.Configurations != null)
        {
            foreach (var config in entity.Configurations)
            {
                if (!string.IsNullOrWhiteSpace(config.SettingKey))
                {
                    // Use indexer to overwrite duplicates instead of throwing
                    configDict[config.SettingKey] = config.SettingValue ?? string.Empty;
                }
            }
        }

        var options = new ExchangeRateProviderOptions
        {
            Name = entity.Name,
            Code = entity.Code,
            Url = entity.Url,
            BaseCurrency = entity.BaseCurrency.Code,
            IsActive = entity.IsActive,
            RequiresAuthentication = entity.RequiresAuthentication,
            Configuration = configDict
        };

        return options;
    }

    /// <summary>
    /// Gets fallback provider options from appsettings.json by provider code.
    /// </summary>
    private ExchangeRateProviderOptions? GetFallbackProviderOptions(string providerCode)
    {
        var config = _fallbackConfig?.Value;
        if (config == null)
        {
            _logger.LogWarning("Fallback configuration is not available");
            return null;
        }

        return providerCode.ToUpperInvariant() switch
        {
            "CNB" => config.CNB,
            "ECB" => config.ECB,
            "BNR" => config.BNR,
            _ => null
        };
    }

    /// <summary>
    /// Gets all fallback provider options from appsettings.json.
    /// </summary>
    private List<ExchangeRateProviderOptions> GetAllFallbackProviderOptions()
    {
        var providers = new List<ExchangeRateProviderOptions>();

        var config = _fallbackConfig?.Value;
        if (config == null)
        {
            _logger.LogWarning("Fallback configuration is not available");
            return providers;
        }

        if (config.CNB != null && !string.IsNullOrWhiteSpace(config.CNB.Code) && config.CNB.IsActive)
            providers.Add(config.CNB);

        if (config.ECB != null && !string.IsNullOrWhiteSpace(config.ECB.Code) && config.ECB.IsActive)
            providers.Add(config.ECB);

        if (config.BNR != null && !string.IsNullOrWhiteSpace(config.BNR.Code) && config.BNR.IsActive)
            providers.Add(config.BNR);

        return providers;
    }
}
