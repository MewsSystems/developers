using Application.ExchangeRateProvider;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationDependencyInjection
    {

        public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
        {

            serviceCollection.AddTransient<IExchangeRateProvider, ExchangeRateProvider.ExchangeRateProvider>();

            return serviceCollection;

        }

    }
}
