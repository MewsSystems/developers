using ExchangeRateUpdater.Services.Client;
using ExchangeRateUpdater.Services.Configuration;
using ExchangeRateUpdater.Services.Implementations;
using ExchangeRateUpdater.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Polly;
using Refit;

namespace ExchangeRateUpdater.Extensions
{
    internal static class HostExtensions
    {
        public static IServiceCollection AddInternalServices(this IServiceCollection services)
        {
            services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();

            return services;
        }

        public static IServiceCollection AddConfiguration(
            this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.Get<AppConfiguration>();
            services.AddSingleton(settings!);
            return services;
        }

        public static IServiceCollection AddRefitClient(
            this IServiceCollection services, IConfiguration configuration)
        {
            var baseUrl = configuration.GetValue<string>("CzechNationalBankApiUrl");

            var backoffBase = configuration.GetValue<int>("BackoffBase");
            var retryTimes = configuration.GetValue<int>("RetryTimes");


            var retryPolicy = Policy
            .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(retryTimes, retryAttempt => TimeSpan.FromSeconds(Math.Pow(backoffBase, retryAttempt)));
            //example for Retry 3 times, using exponential backoff of base 2
            // This means it waits 2^1 = 2 seconds after the first failed attempt,
            // 2^2 = 4 seconds after the second failed attempt,
            // and 2^3 = 8 seconds after the third failed attempt.

            var settings = new RefitSettings
            {
                HttpMessageHandlerFactory = () => new PolicyHttpMessageHandler(retryPolicy)
            };

            services.AddRefitClient<ICzechNationalBankClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl ?? string.Empty));

            return services;
        }
    }
}
