using Microsoft.Extensions.Hosting;
using Serilog;

namespace Common.Log.Extensions;

public static class HostBuilderExtensions
{
    /// <summary>
    ///     Adds serilog with configuration defined in appsettings
    /// </summary>
    /// <param name="hostBuilder"></param>
    /// <returns></returns>
    public static IHostBuilder AddSerilog(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseSerilog((hostContext, config) =>
        {
            config
                .ReadFrom.Configuration(hostContext.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithCorrelationId();
        });
    }
}