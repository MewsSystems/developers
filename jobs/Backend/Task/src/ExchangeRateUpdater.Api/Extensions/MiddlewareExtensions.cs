using ExchangeRateUpdater.Api.Middlewares;

namespace ExchangeRateUpdater.Api.Extensions;

/// <summary>
/// Extension methods for configuring middleware components
/// </summary>
public static class MiddlewareExtensions
{
    /// <summary>
    /// Adds global exception handling middleware to the application pipeline
    /// </summary>
    /// <param name="app">The application builder instance</param>
    /// <returns>The application builder instance for chaining</returns>
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}