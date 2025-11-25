namespace EchangeRateUpdater.Api.Configurations.Extensions;

using Microsoft.OpenApi.Models;

public static class OpenApiConfiguration
{
    public static IServiceCollection AddOpenApiConfiguration(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Info.Title = "Exchange Rate Updater API";
                document.Info.Contact = new OpenApiContact
                {
                    Name = "Amin Ch",
                    Email = "experimentalaminch@outlook.com"
                };

                return Task.CompletedTask;
            });
        });

        return services;
    }
}