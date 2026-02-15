using ExchangeRateUpdater.Abstraction.Interfaces;
using ExchangeRateUpdater.CbnApiClient.Implementation;
using ExchangeRateUpdater.Service;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace ExchangeRateUpdater.DependencyInjection
{
    /// <summary>
    /// Provides extension methods for registering exchange rate client services with the dependency injection
    /// container.
    /// </summary>
    /// <remarks>This static class is intended to be used in application startup to ensure that the
    /// IExchangeRateClient implementation is available for dependency injection. It simplifies the configuration of
    /// exchange rate services by encapsulating the registration logic.</remarks>
    public static class ExchangeRateCnbServicesExtension
    {
        public static IServiceCollection AddExchangeRateServices(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();
            services.AddScoped<IExchangeRateService, ExchangeRateService>();
            services.AddScoped<IExchangeRateClient, ExchangeRateCbnApiClient>();

            // Configure a named HttpClient for the CNB API with Polly-based retry policy.
            // This will be available as httpClientFactory.CreateClient("CnbApi").
            services.AddHttpClient("CnbApi")
                .AddPolicyHandler(HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                );

            return services;
        }
    }
}
