using Microsoft.Extensions.DependencyInjection;

namespace API.Factory
{
    public class ExchangeRateProviderModule
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddHttpClient();

            // Create a dictionary to map provider types to service types
            var providerTypeMap = DynamicDependencyLoader.LoadDependencies();

            // Register the factory itself
            services.AddSingleton<ExchangeRateProviderFactory>(provider =>
                new ExchangeRateProviderFactory(
                    provider.GetRequiredService<IHttpClientFactory>(),
                    providerTypeMap));
        }
    }
}
