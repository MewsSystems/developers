using System.Text;
using ExchangeRateUpdater.Api.Validators;
using ExchangeRateUpdater.Application.Queries.GetExchangeRates;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace ExchangeRateUpdater.Api.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenTelemetry(this IServiceCollection services, string applicationName)
    {
        // TODO: Add more thorough OpenTelemetry instrumentation (HTTP client, runtime metrics, etc.)
        services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("ExchangeRateUpdater.Api"))
                    .AddMeter("ExchangeRateMetrics")
                    .AddOtlpExporter(opt =>
                    {
                        opt.Endpoint = new Uri("http://otel-collector:4317");
                        opt.Protocol = OtlpExportProtocol.Grpc;
                    });
            });

        return services;
    }
    
    public static IServiceCollection AddDistributedRedisCache(
        this IServiceCollection services, 
        string applicationName, 
        string redisConnectionString)
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
    
    public static IServiceCollection AddJwtBearerAuthentication(
        this IServiceCollection services, 
        JwtSettings jwtSettings)
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