using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Application;

public static class Extensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}
