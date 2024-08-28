using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Microsoft.Extensions.DependencyInjection;

public static class HealthChecksConfiguration
{
    private const string _livenessEndpoint = "/health";
    private const string _readinessEndpoint = "/ready";

    public static IServiceCollection ConfigureHealthChecks(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy());

        return services;
    }

    public static void MapCustomHealthChecks(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapHealthChecks(_livenessEndpoint, new HealthCheckOptions
        {
            Predicate = registration => registration.Name.Equals("self")
        });

        endpointRouteBuilder.MapHealthChecks(_readinessEndpoint, new HealthCheckOptions
        {
            Predicate = registration => registration.Tags.Contains("dependencies")
        });
    }
}
