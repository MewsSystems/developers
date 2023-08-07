using Mews.ExchangeRate.Provider.CNB;
using Hellang.Middleware.ProblemDetails;
using System.Diagnostics.CodeAnalysis;
using Mews.ExchangeRate.API.Mapper;
using Mews.ExchangeRate.Domain;

namespace Mews.ExchangeRate.API.ConfigurationExtensionMethods;

[ExcludeFromCodeCoverage(Justification = "This class configures framework and it´s not testable by unit tests")]
public static class IServiceCollectionExtensionMethods
{
    public static IServiceCollection AddMewsServices(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services
        .AddCNBExchangeRateServices(configuration)
        .AddScoped<IProvideExchangeRates, ExchangeRateProvider>()
        .AddProblemDetails(options =>
        {
            options.IncludeExceptionDetails = (context, exception) => true;
        })
        .AddAutoMapper(typeof(DtoToDomainProfile));

        return services;
    }
}
