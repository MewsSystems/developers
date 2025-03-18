using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Configuration;

public static class HealthCheckExtensions
{

    public static void ConfigureHealthCheckEndpoint(this IEndpointRouteBuilder endpoints)
    {
        var logger = endpoints.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("HealthChecks");

        endpoints.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                var result = JsonSerializer.Serialize(new
                {
                    status = report.Status.ToString(),
                    checks = report.Entries.Select(e => new
                    {
                        name = e.Key,
                        status = e.Value.Status.ToString(),
                        description = e.Value.Description
                    }),
                    duration = report.TotalDuration.TotalMilliseconds
                });

                logger.LogInformation("Health Check Result: {result}", result);

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }
        });
    }
}
