using Mews.ExchangeRateUpdater.Application.Interfaces;
using Mews.ExchangeRateUpdater.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Mews.ExchangeRateUpdater.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IGetExchangeRatesUseCase, GetExchangeRatesUseCase>();
        services.AddScoped<IFetchExchangeRatesUseCase, FetchExchangeRatesUseCase>();

        return services;
    }
}