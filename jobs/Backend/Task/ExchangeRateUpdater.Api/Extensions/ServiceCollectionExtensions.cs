using ExchangeRateUpdater.Api.Clients;
using ExchangeRateUpdater.Api.Configuration;
using ExchangeRateUpdater.Api.Services;
using ExchangeRateUpdater.Api.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Polly;
using Polly.Extensions.Http;

namespace ExchangeRateUpdater.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = $"{configuration.GetValue<string>("RedisConfiguration:Host")}:{configuration.GetValue<int>("RedisConfiguration:Port")}";
            });

            services.AddSingleton<ICacheService, CacheService>();

            return services;
        }

        public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            services.Configure<SourceConfiguration>(configuration.GetSection(nameof(SourceConfiguration)));
            services.Configure<RetryConfiguration>(configuration.GetSection(nameof(RetryConfiguration)));

            return services;
        }

        public static IServiceCollection AddCustomValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();

            services.AddValidatorsFromAssemblyContaining<DailyExchangeRatesRequestValidator>();
            return services;
        }

        public static IServiceCollection AddCnbHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            var sourceConfig = configuration.GetSection(nameof(SourceConfiguration)).Get<SourceConfiguration>();
            var retryConfig = configuration.GetSection(nameof(RetryConfiguration)).Get<RetryConfiguration>();

            services
                .AddHttpClient<ICnbClient, CnbClient>(client =>
                    {
                        client.BaseAddress = new Uri(sourceConfig.BaseAddress);
                    })
                .AddPolicyHandler(GetRetryPolicy(retryConfig));

            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(RetryConfiguration retryConfig)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(retryConfig.RetriesCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}
