using System;
using System.Net.Http;
using ExchangeRateUpdater.CnbExchangeRateProvider.ApiClient;
using ExchangeRateUpdater.ConfigurationOptions;
using ExchangeRateUpdater.ExchangeRateProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Caching;
using Polly.Caching.Memory;
using Polly.Extensions.Http;

namespace ExchangeRateUpdater;

public static class HostApplicationExtensions
{
    private const string CachingPolicyName = "CachingPolicy";

    public static void ConfigureServices(this HostApplicationBuilder builder)
    {
        builder.Services.Configure<CnbClientOptions>(builder.Configuration.GetSection(CnbClientOptions.CnbClient));
    }
    public static void AddServices(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton(TimeProvider.System);
        services.AddSingleton<IAsyncCacheProvider, MemoryCacheProvider>();
        
        services.AddHttpClient<ApiClient>((sp, httpClient) =>
            {
                httpClient.BaseAddress = new Uri(sp.GetRequiredService<IConfiguration>()
                    .GetSection(CnbClientOptions.CnbClient).Get<CnbClientOptions>()!.BaseUri);
            })
            .AddPolicyHandlerFromRegistry((policyRegistry, _) =>
                policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>(CachingPolicyName));

        services.AddTransient<IApiClient>(sp => sp.GetRequiredService<ApiClient>());
        services.AddTransient<IExchangeRateProvider, CnbExchangeRateProvider.CnbExchangeRateProvider>();
    }
    
    public static void AddCacheAndRetryPolicy(this HostApplicationBuilder builder)
    {
        builder.Services.AddPolicyRegistry((sp, policyRegistry) =>
        {
            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            var cachePolicy = Policy.WrapAsync(
                retryPolicy,
                Policy.CacheAsync(
                    cacheProvider: sp.GetRequiredService<IAsyncCacheProvider>().AsyncFor<HttpResponseMessage>(),
                    ttlStrategy: new ResultTtl<HttpResponseMessage>(result =>
                        new Ttl(timeSpan: result?.IsSuccessStatusCode ?? false
                            ? TimeSpan.FromSeconds(sp.GetRequiredService<IConfiguration>()
                                .GetSection(CnbClientOptions.CnbClient).Get<CnbClientOptions>()!.CacheLifeTimeInSeconds)
                            : TimeSpan.Zero))
                ));
            
            policyRegistry.Add(CachingPolicyName, cachePolicy);
        });
    }
}