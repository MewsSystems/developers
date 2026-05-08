using ExchangeRateUpdater.Core.Interfaces;
using ExchangeRateUpdater.Infrastructure.Caching;
using ExchangeRateUpdater.Infrastructure.Configuration;
using ExchangeRateUpdater.Infrastructure.DataSources.Cnb;
using ExchangeRateUpdater.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Polly;
using Polly.Extensions.Http;

namespace ExchangeRateUpdater.Infrastructure.Extensions;

/// <summary>
/// Extension methods for registering Infrastructure services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds infrastructure services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        // Configure options
        services.Configure<ExchangeRateServiceOptions>(
            configuration.GetSection(ExchangeRateServiceOptions.SectionName));

        // Configure Redis Cache
        services.AddStackExchangeRedisCache(options => { options.Configuration = configuration["Redis:Configuration"]; });

        // Register infrastructure services
        services.AddScoped<ICacheService, RedisCacheService>();
        services.AddScoped<IExchangeRateService, CnbExchangeRateService>();

        // Register HttpClient for CNB with Polly policies
        services.AddHttpClient<IExchangeRateDataSource, CnbExchangeRateDataSource>()
            .AddPolicyHandler((services,
                request) =>
            {
                var options = services.GetRequiredService<IOptions<ExchangeRateServiceOptions>>()
                    .Value;
                return HttpPolicyExtensions.HandleTransientHttpError()
                    .WaitAndRetryAsync(
                        options.RetryCount,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(options.RetryBaseDelaySeconds, retryAttempt)),
                        (outcome,
                            timespan,
                            retryAttempt,
                            context) =>
                        {
                            services.GetRequiredService<ILogger<CnbExchangeRateDataSource>>()
                                .LogWarning("Retrying CNB API request {Attempt} after {Delay}ms",
                                    retryAttempt,
                                    timespan.TotalMilliseconds);
                        });
            });

        // Configure OpenTelemetry
        var serviceName = configuration["OpenTelemetry:ServiceName"] ?? "ExchangeRateUpdater";
        var otelEndpoint = configuration["OpenTelemetry:Endpoint"] ?? "http://localhost:4317";

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService(serviceName: serviceName))
            .WithTracing(tracing => tracing
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddSource(serviceName)
                .AddOtlpExporter(opts => opts.Endpoint = new Uri(otelEndpoint)))
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation()
                .AddOtlpExporter(opts => opts.Endpoint = new Uri(otelEndpoint)));

        return services;
    }
}