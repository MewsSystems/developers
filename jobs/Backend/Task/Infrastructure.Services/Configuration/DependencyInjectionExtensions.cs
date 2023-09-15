using Infrastructure.Models.Constants;
using Infrastructure.Services.Abstract;
using Infrastructure.Services.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services.Configuration;

public static class DependencyInjectionExtensions
{
    public static void LoadDependencyInjection(this IServiceCollection services)
    {
        services.AddHttpClient<IBankDataService, CzechNationalBankDataService>(client =>
        {
            client.BaseAddress = new Uri(CzechNationalBankApiEndpoints.BaseUrl);
        });
        services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
    }
}
