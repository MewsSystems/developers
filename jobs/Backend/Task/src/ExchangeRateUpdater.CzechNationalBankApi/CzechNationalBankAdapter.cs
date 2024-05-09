using ExchangeRateUpdater.Core.Configuration;
using ExchangeRateUpdater.Core.Providers;
using ExchangeRateUpdater.CzechNationalBank.Api;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.CzechNationalBank
{
    public static class CzechNationalBankAdapter
    {
        internal static readonly string ApiClientName = "CzechNationalBank.Api";

        public static IServiceCollection ConfigureCzechNationalBank(this IServiceCollection service, CzechNationalBankConfiguration configuration)
        {
            service.AddSingleton(configuration);
            service.AddSingleton<IExchangeRateProvider, CzechNationalBankExchangeRateProvider>();
            service.AddSingleton<ICzechNationalBankApi, CzechNationalBankApi>();

            service.AddHttpClient(
                ApiClientName,
                x =>
                {
                    x.BaseAddress = configuration.ApiBaseUrl;
                });

            return service;
        }
    }
}
