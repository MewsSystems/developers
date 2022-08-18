using ExchangeRateUpdater.ExchangeRateApiServiceClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestEase;

namespace ExchangeRateUpdater.Infrastructure
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
            => services.AddExchangeRateApiServiceClient(configuration);

        private static IServiceCollection AddExchangeRateApiServiceClient(this IServiceCollection services, IConfiguration configuration)
        {
            var baseUrl = configuration.GetSection(nameof(IExchangeRateApiServiceClient))["BaseUrl"];
            services.AddSingleton(new RestClient(baseUrl)
                    {
                        JsonSerializerSettings = new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver(),
                            NullValueHandling = NullValueHandling.Ignore
                        }
                    }
                    .For<IExchangeRateApiServiceClient>());
            return services;
        }
    }
}