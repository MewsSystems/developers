namespace ExchangeRateUpdater.Infrastructure;

using System;
using ApiClients;
using Application.Common.Interfaces;
using Configurations;
using Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Data;
using Polly;
using Polly.Extensions.Http;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection self, IConfiguration configuration)
    {
        self.Configure<ExchangeRateApiClientConfig>(configuration.GetSection(nameof(ExchangeRateApiClientConfig)))
            .AddHttpClient<IExchangeRateApiClient, ExchangeRateApiClient>((sp, httpClient) =>
            {
                var exchangeRateApiClientConfig = sp.GetService<IOptions<ExchangeRateApiClientConfig>>();
                httpClient.BaseAddress = new Uri(exchangeRateApiClientConfig.Value.BaseUrl);
            }).AddPolicyHandler(GetRetryPolicy());

        // Redis configuration
        var redisConnection = configuration.GetValue<string>("Redis:Connection");

        if (!string.IsNullOrWhiteSpace(redisConnection))
        {
            var mux = ConnectionMultiplexer.Connect(redisConnection);
            self.AddSingleton<IConnectionMultiplexer>(mux);
            self.AddSingleton<ICacheRepository, RedisCacheRepository>();
        }
        else
        {
            self.AddSingleton<ICacheRepository, CacheRepository>();
            self.AddMemoryCache();
        }

        return self;
    }

    static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}