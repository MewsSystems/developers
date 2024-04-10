using ExchangeRates.Domain.Repositories;
using ExchangeRates.Infrastructure.Mappers;
using ExchangeRates.Infrastructure.Repositories;
using ExchangeRates.Infrastructure.Settings;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace ExchangeRates.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Options.
        var retryPolicySettings = configuration.GetSection(nameof(RetryPolicy)).Get<RetryPolicy>();

        services.Configure<CacheSettings>(configuration.GetSection(nameof(CacheSettings)));
        services.Configure<CzechNationalBankUrls>(configuration
            .GetSection(nameof(ExternalApis))
            .GetSection(nameof(CzechNationalBankUrls)));

        // HTTP.
        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryPolicySettings.RetryAttemptInSeconds));

        services
            .AddHttpClient("PollyClient")
            .AddPolicyHandler(retryPolicy);

        // Cache.
        services.AddDistributedMemoryCache();

        // Custom services.
        services.AddScoped<IExchangeRateMapper, ExchangeRateMapper>();
        services.AddScoped<ExchangeRatesRepository>();
        services.AddScoped<IExchangeRateRepository>(provider =>
        {
            return new CacheExchangeRateRepository(
                    provider.GetRequiredService<IOptions<CacheSettings>>(),
                    provider.GetRequiredService<IDistributedCache>(),
                    provider.GetRequiredService<ExchangeRatesRepository>()
                    );
        });

        return services;
    }
}
