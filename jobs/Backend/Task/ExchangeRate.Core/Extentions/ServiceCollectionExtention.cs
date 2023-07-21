using ExchangeRate.Core.Configuration;
using ExchangeRate.Core.Constants;
using ExchangeRate.Core.ExchangeRateSourceClients;
using ExchangeRate.Core.Models.ClientResponses;
using ExchangeRate.Core.Providers;
using ExchangeRate.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ExchangeRate.Core.Extentions;

public static class ServiceCollectionExtention
{
    public static IServiceCollection AddExchangeRateProvider(this IServiceCollection services, Action<CnbSettings> configureOptions)
    {
        services.Configure(configureOptions);

        services.AddMemoryCache();
        services.AddHttpClient(ExchangeRateSourceCodes.CzechNationalBank, (serviceProvider, client) =>
        {
            var cnbSettings = serviceProvider.GetRequiredService<IOptions<CnbSettings>>().Value;
            client.BaseAddress = new Uri(cnbSettings.ApiBaseUrl);
        });

        services.AddSingleton<IExchangeProviderResolver, ExchangeProviderResolver>();
        services.AddTransient<ExchangeRateProvidersFactory>();
        services.AddTransient<ICacheService, InMemoryCacheService>();
        services.AddTransient<IExchangeRateSourceClient<CnbExchangeRate>, CnbExchangeRateClient>();
        services.AddTransient<CnbExchangeRateProvider>();

        return services;
    }
}
