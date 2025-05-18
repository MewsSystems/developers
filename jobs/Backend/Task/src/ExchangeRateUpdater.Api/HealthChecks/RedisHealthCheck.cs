using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace ExchangeRateUpdater.Api.HealthChecks;

public class RedisHealthCheck : IHealthCheck
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<RedisHealthCheck> _logger;
    private readonly IOptions<Microsoft.Extensions.Caching.StackExchangeRedis.RedisCacheOptions> _redisOptions;

    public RedisHealthCheck(
        IDistributedCache cache,
        ILogger<RedisHealthCheck> logger,
        IOptions<Microsoft.Extensions.Caching.StackExchangeRedis.RedisCacheOptions> redisOptions)
    {
        _cache = cache;
        _logger = logger;
        _redisOptions = redisOptions;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Use a shorter timeout for health checks
            var configurationOptions = ConfigurationOptions.Parse(_redisOptions.Value.Configuration);
            configurationOptions.ConnectTimeout = 2000; // 2 seconds timeout for health checks

            using var connection = await ConnectionMultiplexer.ConnectAsync(configurationOptions);
            var db = connection.GetDatabase();

            var pingResult = await db.PingAsync();
            if (pingResult.TotalMilliseconds < 100) return HealthCheckResult.Healthy($"Redis ping successful: {pingResult.TotalMilliseconds}ms");

            return HealthCheckResult.Degraded($"Redis ping response time degraded: {pingResult.TotalMilliseconds}ms");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Redis health check failed");
            return HealthCheckResult.Unhealthy("Redis connection failed", ex);
        }
    }
}