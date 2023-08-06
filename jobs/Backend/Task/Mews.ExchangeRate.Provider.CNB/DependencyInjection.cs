using Mews.ExchangeRate.Domain;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Mews.ExchangeRate.Provider.CNB;

[ExcludeFromCodeCoverage(Justification = "This class configures framework and it´s not testable by unit tests")]
public static class DependencyInjection
{
    public static IServiceCollection AddCNBExchangeRateServices(this IServiceCollection services)
    {
        return services
            .AddCNBServices()
            .AddCNBHealthChecks();
    }

    private static IServiceCollection AddCNBServices(this IServiceCollection services)
    {
        return services.AddScoped<IProvideExchangeRates, CnbExchangeRateProvider>();
    }

    private static IServiceCollection AddCNBHealthChecks(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddCheck<CnbExchangeRateProvider>(nameof(CnbExchangeRateProvider));

        return services;
    }
}
