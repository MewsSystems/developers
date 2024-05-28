using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System.Net.Http;

namespace ExchangeRateUpdater.ExchangeRateProviders.CzechNationalBank;

public static class ServiceCollectionExtension
{
    public static ServiceCollection RegisterCzechNationalBankProvider(this ServiceCollection services,
        IConfigurationRoot configuration, IAsyncPolicy<HttpResponseMessage> retryPolicy)
    {
        services.AddHttpClient<CzechNationalBankExchangeRateProvider>().AddPolicyHandler(retryPolicy);
        services.AddSingleton<IExchangeRateProvider, CzechNationalBankExchangeRateProvider>();
        services.Configure<CzechNationalBankExchangeRateProviderConfiguration>(configuration.GetSection("CzechNationalBankProvider"));

        return services;
    }
}