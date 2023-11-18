using ExchangeRateUpdater.Api.Clients;
using ExchangeRateUpdater.Api.Configuration;
using Polly;
using Polly.Extensions.Http;

namespace ExchangeRateUpdater.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomOptions(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddOptions();

            serviceCollection.Configure<SourceConfiguration>(configuration.GetSection(nameof(SourceConfiguration)));
            serviceCollection.Configure<RetryConfiguration>(configuration.GetSection(nameof(RetryConfiguration)));

            return serviceCollection;
        }

        public static IServiceCollection AddCnbHttpClient(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var sourceConfig = configuration.GetSection(nameof(SourceConfiguration)).Get<SourceConfiguration>();
            var retryConfig = configuration.GetSection(nameof(RetryConfiguration)).Get<RetryConfiguration>();

            serviceCollection
                .AddHttpClient<ICnbClient, CnbClient>(client =>
                    {
                        client.BaseAddress = new Uri($"{sourceConfig.BaseAddress}/{sourceConfig.DailyExchangeRatesEndpoint}");
                    })
                .AddPolicyHandler(GetRetryPolicy(retryConfig));

            return serviceCollection;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(RetryConfiguration retryConfig)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(retryConfig.RetriesCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

       
    }
}
