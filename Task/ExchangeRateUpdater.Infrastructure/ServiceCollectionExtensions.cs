namespace ExchangeRateUpdater.Infrastructure;

using System;
using ApiClients;
using Application.Common.Interfaces;
using Cache;
using Configurations;
using Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using StackExchange.Redis;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection self, IConfiguration configuration)
    {
        self.Configure<ExchangeRateApiClientConfig>(configuration.GetSection(nameof(ExchangeRateApiClientConfig)))
            .AddHttpClient<IExchangeRateApiClient, ExchangeRateApiClient>((sp, httpClient) =>
            {
                var exchangeRateApiClientConfig = sp.GetService<IOptions<ExchangeRateApiClientConfig>>() ??
                                                  throw new NullReferenceException($"{nameof(ExchangeRateApiClientConfig)} is not configured");
                httpClient.BaseAddress = new Uri(exchangeRateApiClientConfig.Value.BaseUrl);
            }).AddPolicyHandler(GetRetryPolicy());
        
        var redisConnection = configuration.GetValue<string>("Redis:Connection");

        if (string.IsNullOrWhiteSpace(redisConnection))
        {
            throw new NullReferenceException("Redis:Connection setting is not configured");
        }
        
        self.AddScoped<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(redisConnection));
        self.AddScoped<ICacheRepository, RedisCacheRepository>();

        return self;
    }

    static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}