using ExchangeRateUpdater.Application.Handlers.QueryHandlers.Abstract;
using ExchangeRateUpdater.Application.Handlers.QueryHandlers.GetSupportedExchangeRates;
using ExchangeRateUpdater.Domain.Entities;

namespace Microsoft.Extensions.DependencyInjection;

public static class HandlersConfiguration
{
    public static IServiceCollection ConfigureHandlers(this IServiceCollection services)
    {
        return services
            .ConfigureQueryHandlers();
    }

    private static IServiceCollection ConfigureQueryHandlers(this IServiceCollection services)
    {
        return services
            .AddScoped<IAsyncQueryHandler<IEnumerable<ExchangeRate>>, GetSupportedExchangeRatesQueryHandler>();
    }
}
