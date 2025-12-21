using ExchangeRateProvider.Api.Constants;
using System.Threading.RateLimiting;

namespace ExchangeRateProvider.Api.Extensions;

/// <summary>
/// Provides extension methods for configuring rate limiting for the exchange rates API.
/// </summary>
public static class RateLimitingExtensions
{
    /// <summary>
    /// Adds rate limiting configuration for exchange rates endpoints with token bucket algorithm.
    /// Uses client-based limiting for authenticated requests (via 'azp' or 'sub' claims) and IP-based
    /// limiting for anonymous requests. Allows 60 requests per minute with no queuing, returning
    /// HTTP 429 when limit is exceeded.
    /// </summary>
    /// <param name="services">The service collection to add rate limiting services to.</param>
    /// <returns>The service collection for method chaining.</returns>
    public static IServiceCollection AddExchangeRatesRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.AddPolicy(RateLimitPolicies.ExchangeRates, context =>
            {
                var clientId = context.User.FindFirst("azp")?.Value ??
                               context.User.FindFirst("sub")?.Value;

                var key = !string.IsNullOrWhiteSpace(clientId)
                    ? $"client:{clientId}"
                    : $"ip:{context.Connection.RemoteIpAddress}";

                return RateLimitPartition.GetTokenBucketLimiter(key, _ => new TokenBucketRateLimiterOptions
                {
                    TokenLimit = 60,
                    TokensPerPeriod = 60,
                    ReplenishmentPeriod = TimeSpan.FromMinutes(1),
                    AutoReplenishment = true,
                    QueueLimit = 0
                });
            });
        });

        return services;
    }
}
