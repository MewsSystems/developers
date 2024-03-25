using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateDemo.Infrastructure.Providers
{
    public static class BaseRestProvider
    {
        public static IHttpClientBuilder AddRestProvider<TInterface, TImplementation>(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient) where TInterface : class, IRestProvider where TImplementation : RestProvider, TInterface
        {
            IHttpClientBuilder builder = services.AddHttpClient<TInterface, TImplementation>(configureClient);
            services.AddTransient<TInterface, TImplementation>(delegate (IServiceProvider provider)
            {
                TImplementation imp = ActivatorUtilities.CreateInstance<TImplementation>(provider, []);
                HttpClient client = (imp.Client = ServiceProviderServiceExtensions.GetRequiredService<IHttpClientFactory>(provider).CreateClient(builder.Name));
                imp.Client = client;
                return imp;
            });

            return builder;
        }
    }
}
