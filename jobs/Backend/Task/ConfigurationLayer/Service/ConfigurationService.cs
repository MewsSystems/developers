using ConfigurationLayer.Interface;
using ConfigurationLayer.Option;
using Microsoft.EntityFrameworkCore;
using DataLayer;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationLayer.Service
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly IOptions<SystemConfigurationOptions> _fallbackConfig;
        private readonly IMemoryCache _cache;
        private readonly ILogger<ConfigurationService> _logger;
        private const int CacheExpirationMinutes = 15;

        public ConfigurationService(
            IDbContextFactory<ApplicationDbContext> contextFactory,
            IOptions<SystemConfigurationOptions> fallbackConfig,
            IMemoryCache cache,
            ILogger<ConfigurationService> logger)
        {
            _contextFactory = contextFactory;
            _fallbackConfig = fallbackConfig;
            _cache = cache;
            _logger = logger;
        }

        public async Task<string> GetValueAsync(string key, string defaultValue = "")
        {
            // Check cache first
            var cacheKey = $"config_{key}";
            if (_cache.TryGetValue<string>(cacheKey, out var cachedValue) && cachedValue != null)
            {
                _logger.LogDebug("Configuration key '{Key}' retrieved from cache", key);
                return cachedValue;
            }

            try
            {
                // Try database first
                await using var context = await _contextFactory.CreateDbContextAsync();
                var config = await context.SystemConfigurations
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Key == key);

                if (config != null)
                {
                    _logger.LogDebug("Configuration key '{Key}' retrieved from database", key);

                    // Cache the value
                    _cache.Set(cacheKey, config.Value, TimeSpan.FromMinutes(CacheExpirationMinutes));
                    return config.Value;
                }

                _logger.LogDebug("Configuration key '{Key}' not found in database, using fallback", key);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to read configuration key '{Key}' from database, using fallback", key);
            }

            // Fallback to appsettings.json
            var fallbackValue = GetFallbackValue(key);

            if (fallbackValue != null)
            {
                _logger.LogDebug("Configuration key '{Key}' retrieved from appsettings.json fallback", key);

                // Cache the fallback value too
                _cache.Set(cacheKey, fallbackValue, TimeSpan.FromMinutes(CacheExpirationMinutes));
                return fallbackValue;
            }

            _logger.LogDebug("Configuration key '{Key}' not found, using default value: {DefaultValue}", key, defaultValue);
            return defaultValue;
        }

        public async Task<T?> GetValueAsync<T>(string key, T? defaultValue = default)
        {
            var stringValue = await GetValueAsync(key);

            if (string.IsNullOrEmpty(stringValue))
                return defaultValue;

            try
            {
                // Handle bool specially
                if (typeof(T) == typeof(bool))
                {
                    if (bool.TryParse(stringValue, out var boolResult))
                        return (T)(object)boolResult;

                    // Handle "true"/"false" strings case-insensitively
                    return (T)(object)stringValue.Equals("true", StringComparison.OrdinalIgnoreCase);
                }

                // Handle int
                if (typeof(T) == typeof(int))
                {
                    if (int.TryParse(stringValue, out var intResult))
                        return (T)(object)intResult;
                }

                // Handle decimal
                if (typeof(T) == typeof(decimal))
                {
                    if (decimal.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var decimalResult))
                        return (T)(object)decimalResult;
                }

                // Handle DateTime
                if (typeof(T) == typeof(DateTime))
                {
                    if (DateTime.TryParse(stringValue, out var dateResult))
                        return (T)(object)dateResult;
                }

                // Generic conversion
                return (T)Convert.ChangeType(stringValue, typeof(T), CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to convert configuration value '{Value}' to type {Type} for key '{Key}'",
                    stringValue, typeof(T).Name, key);
                return defaultValue;
            }
        }

        public async Task<bool> GetBoolAsync(string key, bool defaultValue = false)
        {
            return await GetValueAsync<bool>(key, defaultValue);
        }

        public async Task<int> GetIntAsync(string key, int defaultValue = 0)
        {
            return await GetValueAsync<int>(key, defaultValue);
        }

        public async Task<decimal> GetDecimalAsync(string key, decimal defaultValue = 0m)
        {
            return await GetValueAsync<decimal>(key, defaultValue);
        }

        private string? GetFallbackValue(string key)
        {
            var config = _fallbackConfig.Value;

            // Map database configuration keys to appsettings.json values
            return key switch
            {
                // Provider Health
                "AutoDisableProviderAfterFailures" => config.ProviderHealth.AutoDisableAfterFailures.ToString(),
                "ProviderHealthCheckIntervalMinutes" => config.ProviderHealth.HealthCheckIntervalMinutes.ToString(),
                "StaleDataThresholdHours" => config.ProviderHealth.StaleDataThresholdHours.ToString(),

                // Data Retention
                "RetainExchangeRatesDays" => config.DataRetention.RetainExchangeRatesDays.ToString(),
                "RetainFetchLogsDays" => config.DataRetention.RetainFetchLogsDays.ToString(),
                "RetainErrorLogsDays" => config.DataRetention.RetainErrorLogsDays.ToString(),
                "EnableAutoCleanup" => config.DataRetention.EnableAutoCleanup.ToString(),

                // Logging
                "LogLevel" => config.Logging.LogLevel,
                "EnableDetailedLogging" => config.Logging.EnableDetailedLogging.ToString(),
                "LogSuccessfulFetches" => config.Logging.LogSuccessfulFetches.ToString(),

                // API
                "ApiRateLimitPerMinute" => config.Api.RateLimitPerMinute.ToString(),
                "EnableApiKeyAuthentication" => config.Api.EnableApiKeyAuthentication.ToString(),
                "MaxResultsPerPage" => config.Api.MaxResultsPerPage.ToString(),

                // System Information
                "SystemVersion" => config.System.Version,
                "MaintenanceMode" => config.System.MaintenanceMode.ToString(),
                "MaintenanceMessage" => config.System.MaintenanceMessage,

                // Background Jobs
                "HistoricalDataDays" => config.BackgroundJobs.HistoricalDataDays.ToString(),
                "DefaultRetryDelayMinutes" => config.BackgroundJobs.DefaultRetryDelayMinutes.ToString(),
                "MaxRetries" => config.BackgroundJobs.MaxRetries.ToString(),
                "HealthCheckIntervalMinutes" => config.BackgroundJobs.HealthCheckIntervalMinutes.ToString(),
                "RecentDataThresholdHours" => config.BackgroundJobs.RecentDataThresholdHours.ToString(),
                "HangfireWorkerCount" => config.BackgroundJobs.HangfireWorkerCount.ToString(),
                "DefaultCronExpression" => config.BackgroundJobs.DefaultCronExpression,
                "DefaultTimezone" => config.BackgroundJobs.DefaultTimezone,

                // Not found
                _ => null
            };
        }
    }
}
