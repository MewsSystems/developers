using Asp.Versioning;

namespace ExchangeRateUpdater.API.Configuration;

public static class OpenApiConfiguration
{
    public static IServiceCollection AddOpenApiAndSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true; 
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }
}