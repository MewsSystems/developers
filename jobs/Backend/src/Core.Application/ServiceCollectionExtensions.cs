using Core.Application.Interfaces;
using Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();

            return serviceCollection;
        }
    }
}
