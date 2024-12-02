using Mews.ExchangeRate.Domain;
using Mews.ExchangeRate.Provider.CNB.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using Mews.ExchangeRate.Provider.CNB.Mapper;

namespace Mews.ExchangeRate.Provider.CNB;

[ExcludeFromCodeCoverage(Justification = "This class configures framework and it´s not testable by unit tests")]
public static class DependencyInjection
{
    public static IServiceCollection AddCNBExchangeRateServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddCNBServices()
            .AddCNBHealthChecks()
            .AddCNBSettings(configuration)
            .AddMappingProfiles();
    }

    private static IServiceCollection AddCNBServices(this IServiceCollection services)
    {
        services.AddHttpClient();
        return services.AddScoped<IRetrieveExchangeRatesFromSource, CnbExchangeRateProvider>();
    }

    private static IServiceCollection AddCNBHealthChecks(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddCheck<CnbExchangeRateProvider>(nameof(CnbExchangeRateProvider));

        return services;
    }

    private static IServiceCollection AddCNBSettings(this IServiceCollection services,
        IConfiguration configuration) 
    {
        services
            .AddOptions<CnbSettings>()
            .Bind(configuration.GetSection(CnbSettings.Property))
            .ValidateOnStart();

        services
            .AddSingleton<IValidateOptions<CnbSettings>, CnbSettingsValidator>();
        return services;
    }

    private static IServiceCollection AddMappingProfiles(this IServiceCollection services)
    {
        return services.AddAutoMapper(typeof(DtoToDomainProfile));
    }
}
