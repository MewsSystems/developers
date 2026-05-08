using Mews.ExchangeRateMonitor.Common.API.ErrorHandling;
using Mews.ExchangeRateMonitor.Common.API.RateLimiting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

namespace Mews.ExchangeRateMonitor.Common.API;

public static class DependencyInjection
{
    /// <summary>
    /// Adds core web API infrastructure services
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCoreWebApiInfrastructure(this IServiceCollection services)
    {
        services.AddSwagger();
        services.AddRateLimiting();
        services.AddGlobalExceptionHadnler();
        services.AddJsonConfigurations();
        services.AddCors(o => o.AddPolicy("dev", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
        return services;
    }

    /// <summary>
    /// Adds global exception handler and problem details services
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddGlobalExceptionHadnler(this IServiceCollection services)
    {
        services
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddProblemDetails();

        return services;

    }

    /// <summary>
    /// Adds rate limiting policy
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter(AppRateLimiting.GlobalRateLimitPolicy, opt =>
            {
                opt.Window = TimeSpan.FromMinutes(1);    // Time window of 1 minute
                opt.PermitLimit = 100;                   // Allow 100 requests per minute
                opt.QueueLimit = 2;                      // Queues 2 additional requests if the limit is reached
                opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });
        });

        return services;
    }

    /// <summary>
    /// Configures Swagger generation 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });

        return services;
    }

    /// <summary>
    /// REgiser JSON configurations, like enum to string conversion
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddJsonConfigurations(this IServiceCollection services)
    {
        services.Configure<JsonOptions>(opt =>
        {
            opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        return services;
    }

    /// <summary>
    /// Adds logging to the web application builder.
    /// </summary>
    /// <param name="builder"></param>
    public static void AddCoreHostLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, loggerConfig) =>
            loggerConfig.ReadFrom.Configuration(context.Configuration));
    }

    /// <summary>
    /// Adds swagger and swagger UI
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseSwaggerExt(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    }

    /// <summary>
    /// Adds rate limiting
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseAppRateLimiter(this IApplicationBuilder app)
    {
        app.UseRateLimiter();
        return app;
    }
}
