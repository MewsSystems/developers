using ExchangeRateProvider.Api.Middleware;
using Serilog;

namespace ExchangeRateProvider.Api.Extensions;

/// <summary>
/// Extension methods for configuring logging in the application.
/// </summary>
public static class LoggingExtensions
{
    /// <summary>
    /// Configures Serilog as the logging provider for the application.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The web application builder for method chaining.</returns>
    public static WebApplicationBuilder AddSerilogLogging(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Host.UseSerilog();

        return builder;
    }

    /// <summary>
    /// Adds request/response logging middleware to the application pipeline.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The web application for method chaining.</returns>
    public static WebApplication UseRequestResponseLogging(this WebApplication app)
    {
        app.UseMiddleware<RequestResponseLoggingMiddleware>();
        return app;
    }
}