using ExchangeRateFinder.API.Mappers;
namespace ExchangeRateFinder.API.Extensions
{
    public static class DependencyInjection
    {
        public static void AddApi(this IServiceCollection services)
        {
            services.AddTransient<ICalculatedExchangeRateResponseMapper, CalculatedExchangeRateResponseMapper>();
        }
    }
}
