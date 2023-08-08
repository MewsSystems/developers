using Hellang.Middleware.ProblemDetails;
using Mews.ExchangeRate.API.Mappers;
using Mews.ExchangeRate.Domain;
using Mews.ExchangeRate.Provider.CNB;
using System.Diagnostics.CodeAnalysis;

namespace Mews.ExchangeRate.API.ConfigurationExtensionMethods;

[ExcludeFromCodeCoverage(Justification = "This class configures framework and it´s not testable")]
public static class DependencyInjection
{
    public static IServiceCollection AddMewsServicesAndSettings(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddMewsServices(configuration)
            .AddProblemDetails(ConfigureProblemDetails)
            .AddAutoMapper(typeof(DtoToDomainProfile));

        return services;
    }

    private static IServiceCollection AddMewsServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddCNBExchangeRateServices(configuration)
            .AddScoped<IProvideExchangeRates, ExchangeRateProvider>();

        return services;
    }

    private static void ConfigureProblemDetails(Hellang.Middleware.ProblemDetails.ProblemDetailsOptions options)
    {
        options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
        options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);
        options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
    }
}
