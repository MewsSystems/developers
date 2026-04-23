using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;

namespace Mews.ExchangeRateUpdater.Api;

/// <summary>
/// Extensions for <see cref="IServiceCollection"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add swagger documentation.
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        const string contactUrl = "http://www.undefined.com";
        const string contactName = "MEWS";
        const string docNameV1 = "v1";
        const string docInfoTitle = "MEWS ExchangeRateUpdater API";
        const string docInfoVersion = "1.0.0";
        const string docInfoDescription = "MEWS ExchangeRateUpdater API";

        _ = services.AddSwaggerGen(swaggerGenOpts =>
        {
            var contact = new OpenApiContact { Name = contactName, Url = new Uri(contactUrl) };
            swaggerGenOpts.SwaggerDoc(docNameV1,
                new OpenApiInfo
                {
                    Title = docInfoTitle,
                    Version = docInfoVersion,
                    Description = docInfoDescription,
                    TermsOfService = new Uri(contactUrl),
                    Contact = contact
                });

            swaggerGenOpts.DescribeAllParametersInCamelCase();
            swaggerGenOpts.DocInclusionPredicate((_, _) => true);
        });

        return services;
    }
}
