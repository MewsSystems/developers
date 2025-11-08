using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Infrastructure.Providers;
using ExchangeRateUpdater.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace ExchangeRateUpdater.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExchangeRateInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var exchangeRateOptions = configuration.GetSection(ExchangeRateOptions.SectionName).Get<ExchangeRateOptions>() ?? new ExchangeRateOptions();

        services.AddHttpClient<CzechNationalBankProvider>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<CzechNationalBankOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
        })
        .AddPolicyHandler(GetRetryPolicy(exchangeRateOptions))
        .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(exchangeRateOptions.RequestTimeout));

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
