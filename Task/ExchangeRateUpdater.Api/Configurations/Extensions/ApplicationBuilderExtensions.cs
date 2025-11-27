namespace EchangeRateUpdater.Api.Configurations.Extensions;

using Scalar.AspNetCore;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseApiDocumentation(this WebApplication app)
    {
        var enableApiDocumentation = app.Configuration.GetValue<bool>("ApiDocumentation:Enabled", false);
        if (!enableApiDocumentation)
        {
            return app;
        }

        app.MapOpenApi();
        app.MapScalarApiReference((options, _) =>
        {
            options
                .AddPreferredSecuritySchemes("Bearer")
                .WithTitle("Exchange Rates Updater API")
                .WithDarkMode(false)
                .WithLayout(ScalarLayout.Classic)
                .WithDefaultHttpClient(ScalarTarget.Shell, ScalarClient.Curl)
                .WithTheme(ScalarTheme.Kepler)
                .WithModels(false)
                .WithDefaultOpenAllTags(false);
        });

        return app;
    }
}
