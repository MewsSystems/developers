using MewsFinance.Application.Interfaces;
using MewsFinance.Application.Mappings;
using MewsFinance.Application.UseCases.ExchangeRates.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace MewsFinance.Application.DependencyInjection
{
    public static class ServiceCollection
    {
        public static void AddApplicationLayer(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddAutoMapper(typeof(ApplicationProfile));
            serviceCollection.AddScoped<IGetExchangeRatesUseCase, GetExchangeRatesUseCase>();
        }
    }
}
