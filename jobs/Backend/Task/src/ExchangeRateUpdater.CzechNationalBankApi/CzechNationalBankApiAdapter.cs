using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.CzechNationalBankApi
{
    public static class CzechNationalBankApiAdapter
    {
        public static IServiceCollection ConfigureApi(this IServiceCollection service)
        {
            return service;
        }
    }
}
