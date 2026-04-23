using ExchangeRateUpdater.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using System;

namespace ExchangeRateUpdater.Providers.CNB;

public static class CNBServiceCollectionExtensions
{
    public static IServiceCollection AddCNBExchangeRateProvider(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<CNBExchangeRateProviderOptions>()
            .Bind(configuration.GetSection("CNBExchangeRateProvider"))
            .ValidateOnStart();

        services.PostConfigure<CNBExchangeRateProviderOptions>(opt =>
        {
            if (!Uri.TryCreate(opt.BaseUrl, UriKind.Absolute, out _))
                throw new InvalidOperationException("CNB BaseUrl is not a valid absolute URI.");
        });

        services.AddMemoryCache();

        services.AddScoped<ICNBExchangeRateApiClient, CNBExchangeRateApiClient>();

        services.AddKeyedScoped<IExchangeRateProvider, CNBCurrencyExchangeProvider>(Constants.CNBExchangeRateProviderName);

        services.AddHttpClient<ICNBExchangeRateApiClient, CNBExchangeRateApiClient>()
            .ConfigureHttpClient((serviceProvider, client) =>
            {
                var options = serviceProvider
                    .GetRequiredService<IOptions<CNBExchangeRateProviderOptions>>()
                    .Value;

                client.BaseAddress = new Uri(options.BaseUrl);
            })
            .AddStandardResilienceHandler(options =>
            {
                options.Retry.MaxRetryAttempts = 3;
                options.Retry.Delay = TimeSpan.FromSeconds(2);
                options.Retry.BackoffType = DelayBackoffType.Exponential;
                options.Retry.UseJitter = true;

                options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(30);
                options.CircuitBreaker.FailureRatio = 0.5;
                options.CircuitBreaker.MinimumThroughput = 10;
                options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(30);

                options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(10);
                options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(60);
            });

        return services;
    }
}

public sealed record CNBExchangeRateProviderOptions
{
    public required string Name { get; init; }
    public required string BaseUrl { get; init; }
    public TimeSpan CacheAbsoluteExpiration { get; init; } = TimeSpan.FromHours(1);
    public TimeSpan CacheSlidingExpiration { get; init; } = TimeSpan.FromMinutes(30);
}

