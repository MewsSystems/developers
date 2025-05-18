using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace ExchangeRateUpdater.Api.HealthChecks;

/// <summary>
/// Health check implementation for Redis cache
/// </summary>
public class RedisHealthCheck : IHealthCheck
{
    private readonly ILogger<RedisHealthCheck> _logger;
    private readonly IOptions<Microsoft.Extensions.Caching.StackExchangeRedis.RedisCacheOptions> _redisOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="RedisHealthCheck"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging health check operations</param>
    /// <param name="redisOptions">The Redis cache configuration options</param>
    public RedisHealthCheck(
        ILogger<RedisHealthCheck> logger,
        IOptions<Microsoft.Extensions.Caching.StackExchangeRedis.RedisCacheOptions> redisOptions)
    {
        _logger = logger;
        _redisOptions = redisOptions;
    }

    /// <summary>
    /// Checks the health of the Redis cache by performing a ping operation
    /// </summary>
    /// <param name="context">The health check context</param>
    /// <param name="cancellationToken">A token that can be used to cancel the health check</param>
    /// <returns>A task that represents the asynchronous health check operation</returns>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Use a shorter timeout for health checks
            var configurationOptions = ConfigurationOptions.Parse(_redisOptions.Value.Configuration ?? throw new InvalidOperationException("Redis configuration is not set"));

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