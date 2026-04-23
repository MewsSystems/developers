using ExchangeRateUpdater.Api.Presentation.Options;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Api.Presentation.Extensions;

public static class OpenApiExtensions
{
    public static IServiceCollection AddOpenApiDocumentation(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<OpenApiOptions>(
            configuration.GetSection(OpenApiOptions.SectionName));

        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, _) =>
            {
                var openApiOptions = context.ApplicationServices
                    .GetRequiredService<IOptions<OpenApiOptions>>().Value;

                document.Info.Title = openApiOptions.Title;
                document.Info.Version = openApiOptions.Version;
                document.Info.Description = openApiOptions.Description;

                return Task.CompletedTask;
            });
        });

        return services;
    }
}
