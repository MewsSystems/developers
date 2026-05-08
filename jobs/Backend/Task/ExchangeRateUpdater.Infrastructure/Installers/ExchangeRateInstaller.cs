using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Infrastructure.Providers;
using ExchangeRateUpdater.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace ExchangeRateUpdater.Infrastructure.Installers;

public static class ExchangeRateInstaller
{
    public static IServiceCollection AddExchangeRateServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure options
        services.Configure<ExchangeRateOptions>(configuration.GetSection(ExchangeRateOptions.SectionName));
        services.Configure<CzechNationalBankOptions>(configuration.GetSection(CzechNationalBankOptions.SectionName));
        
        var exchangeRateOptions = configuration.GetSection(ExchangeRateOptions.SectionName).Get<ExchangeRateOptions>() 
            ?? new ExchangeRateOptions();

        // Configure CNB Provider with resilience policies
        services.AddHttpClient<CzechNationalBankProvider>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<CzechNationalBankOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
        })
        .AddPolicyHandler(GetRetryPolicy(exchangeRateOptions))
        .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(exchangeRateOptions.RequestTimeout));

        // Register services
        services.AddScoped<IExchangeRateProvider, CzechNationalBankProvider>();
        services.AddScoped<IExchangeRateService, ExchangeRateService>();

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(ExchangeRateOptions options)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                options.MaxRetryAttempts,
                retryAttempt => retryAttempt * options.RetryDelay,
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    Console.WriteLine($"Retry {retryCount} after {timespan.TotalMilliseconds}ms due to: {outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString()}");
                });
    }
}