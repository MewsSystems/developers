using System.Diagnostics.Metrics;
using System.Text;
using ExchangeRateUpdater.Api.Validators;
using ExchangeRateUpdater.Application.Queries.GetExchangeRates;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace ExchangeRateUpdater.Api.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenTelemetry(this IServiceCollection services, string applicationName)
    {
        var exchangeRatesMeter = new Meter("ExchangeRatesHandler", "1.0.0");
        
        var otel = services.AddOpenTelemetry();

        otel.ConfigureResource(resource => resource
            .AddService(serviceName: applicationName));

        otel.WithMetrics(metrics => metrics
            .AddAspNetCoreInstrumentation()
            .AddMeter(exchangeRatesMeter.Name)
            .AddMeter("Microsoft.AspNetCore.Hosting")
            .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
            .AddMeter("System.Net.Http")
            .AddMeter("System.Net.NameResolution")
            .AddPrometheusExporter());

        return services;
    }
    
    public static IServiceCollection AddDistributedRedisCache(this IServiceCollection services, string applicationName, string redisConnectionString)
    {
        services.AddOutputCache()
            .AddStackExchangeRedisCache(x =>
            {
                x.InstanceName = applicationName;
                x.Configuration = redisConnectionString;
            });
        
        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<GetExchangeRatesRequestValidator>();

        return services;
    }

    public static IServiceCollection AddMediatr(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetExchangeRatesQuery).Assembly));

        return services;
    }
    
    public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services, JwtSettings jwtSettings)
    {
        services.AddAuthorization();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                };
            });

        return services;
    }
}