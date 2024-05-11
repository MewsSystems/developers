using ExchangeRateUpdater.Infrastructure.ExternalAPIs.Clients;
using ExchangeRateUpdater.Infrastructure.ExternalAPIs.Options;
using ExchangeRateUpdater.Infrastructure.ExternalAPIs.Providers;
using ExchangeRateUpdater.Infrastructure.Interface.ExternalAPIs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace ExchangeRateUpdater.Infrastructure.ExternalAPIs.Extensions;

public static class ServiceDependenciesExtensions
{
    public static void AddExternalApiInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var providerOptions = configuration.GetSection(Constants.HttpClientOptionsSectionName).Get<ExchangeRateProviderOptions>();
        if (providerOptions is null) throw new InvalidOperationException("Could not find required configuration for Exchange Rate Provider Http Client");
        
        services.AddHttpClient<IExchangeRateClient, ExchangeRateClient>(configureClient: client =>
        {
            client.BaseAddress = new Uri(providerOptions.BaseUrl);
        })
        .AddResilienceHandler(Constants.HttpClientResilience, builder => ConfigureResiliencePipeline(builder, providerOptions));

        services.Configure<ExchangeRateProviderCachingOptions>(configuration.GetSection(Constants.CachingOptionsSectionName));
        services.AddExchangeRateProvider(configuration);
        services.AddMemoryCache();
    }

    private static void ConfigureResiliencePipeline(ResiliencePipelineBuilder<HttpResponseMessage> builder, ExchangeRateProviderOptions providerOptions)
    {
        builder.AddRetry(new RetryStrategyOptions<HttpResponseMessage>()
        {
            MaxRetryAttempts = providerOptions.RetryAttemptsPerCall,
            Delay = providerOptions.RetryInterval,
            UseJitter = true,
            BackoffType = DelayBackoffType.Exponential
        });

        builder.AddCircuitBreaker(new CircuitBreakerStrategyOptions<HttpResponseMessage>()
        {
            BreakDuration = providerOptions.CircuitBreakDuration,
            
        });
            
        builder.AddTimeout(providerOptions.Timeout);
    }
    
    private static void AddExchangeRateProvider(this IServiceCollection services, IConfiguration configuration)
    {
        var cacheOptions = configuration.GetSection(Constants.CachingOptionsSectionName).Get<ExchangeRateProviderCachingOptions>();
        if (cacheOptions is not null && cacheOptions.Enabled)
        {
            services.AddScoped<IExchangeRateProvider, CachingExchangeRateProvider>();
        }
        else
        {
            services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();
        }
    }
}