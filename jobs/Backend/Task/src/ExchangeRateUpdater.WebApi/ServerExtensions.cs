using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Net.Http;

namespace ExchangeRateUpdater.WebApi
{
    public static class ServerExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services,
            string title,
            string description,
            string version = "1")
        {
            return services
                .AddEndpointsApiExplorer()
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc($"v{version}", new OpenApiInfo
                    {
                        Title = title,
                        Version = $"v{version}",
                        Description = description
                    });
                });
        }

        public static IApplicationBuilder UserErrorHandler(this WebApplication app,
           bool showDetailedErrors)
        {
            app.UseExceptionHandler("/error");
            app.Map("/error", (HttpContext httpContext) =>
            {
                var exceptionHandlerFeature = httpContext?.Features.Get<IExceptionHandlerFeature>()!;
                
                if (exceptionHandlerFeature?.Error is BadHttpRequestException)
                    return Results.Problem(
                        type: "validation_error",
                        statusCode: StatusCodes.Status400BadRequest,
                        detail: exceptionHandlerFeature.Error.Message,
                        title: "Invalid request");

                if (exceptionHandlerFeature?.Error is HttpRequestException)
                    return Results.Problem(
                        type: "internal_server_error",
                        statusCode: StatusCodes.Status500InternalServerError,
                        detail: exceptionHandlerFeature.Error.Message,
                        title: "Internal Server Error");

                if (showDetailedErrors)
                    return Results.Problem(
                        statusCode: StatusCodes.Status500InternalServerError,
                        type: "internal_server_error",
                        detail: exceptionHandlerFeature?.Error.StackTrace,
                        title: exceptionHandlerFeature?.Error.Message);                        

                return Results.Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    type: "internal_server_error",
                    title: "Internal Server Error");
            })
            .ExcludeFromDescription();

            return app;
        }
    }
}
