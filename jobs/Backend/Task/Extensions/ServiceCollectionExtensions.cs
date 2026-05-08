using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Services.Builders;
using ExchangeRateUpdater.Services.Clients;
using ExchangeRateUpdater.Services.Parsers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Contrib.WaitAndRetry;
using System;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace ExchangeRateUpdater.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExchangeRateServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging(builder =>
        {
            builder.AddConfiguration(configuration.GetSection("Logging"));
            builder.AddConsole();
        });

        services.Configure<CnbApiConfiguration>(configuration.GetSection("CnbApi"));
        var cnbConfig = configuration.GetSection("CnbApi").Get<CnbApiConfiguration>();

        services.AddHttpClient<ICnbApiClient, CnbApiClient>((serviceProvider, client) =>
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Text.Plain));
        })
        .ConfigureHttpClient(client => client.Timeout = TimeSpan.FromSeconds(cnbConfig.TimeoutSeconds))
        .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder
                .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), cnbConfig.RetryCount)));

        services.AddSingleton<ICnbDataParser, CnbDataParser>();
        services.AddSingleton<IExchangeRateBuilder, ExchangeRateBuilder>();
        services.AddScoped<ExchangeRateProvider>();

        return services;
    }
}
