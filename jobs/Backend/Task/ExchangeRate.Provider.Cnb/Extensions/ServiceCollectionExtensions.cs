using ExchangeRate.Provider.Base.Interfaces;
using ExchangeRate.Provider.Cnb.HttpClient;
using ExchangeRate.Provider.Cnb.Interfaces;
using ExchangeRate.Provider.Cnb.Models.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ExchangeRate.Provider.Cnb.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCnbProviderConfiguration(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var section = configuration.GetSection("ExchangeProviders").GetSection("Cnb");

        serviceCollection.Configure<CnbProviderConfiguration>(section);

        serviceCollection.AddSingleton<IValidatable, CnbProviderConfiguration>(resolver =>
            resolver.GetRequiredService<IOptions<CnbProviderConfiguration>>().Value);

        serviceCollection.AddHttpClient<ICnbHttpClient, CnbHttpClient>();
        serviceCollection.AddScoped<ICnbHttpClient, CnbHttpClient>();

        return serviceCollection;
    }
}