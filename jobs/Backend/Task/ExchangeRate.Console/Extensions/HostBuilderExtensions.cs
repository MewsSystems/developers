using Common.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Environments = Common.Base.Environments;

namespace ExchangeRate.Console.Extensions;

public static class HostBuilderExtensions
{
    /// <summary>
    ///     Checks all environment variables. Set default values for those that are not set.
    /// </summary>
    public static IHostBuilder ConfigureAppEnvironmentVariables(this IHostBuilder hostBuilder)
    {
        #region Application

        var applicationEnvironmentVariable = Environment.GetEnvironmentVariable(EnvironmentVariables.AppEnvironment);

        if (applicationEnvironmentVariable is not null && (
                applicationEnvironmentVariable.Equals(Environments.Production) ||
                applicationEnvironmentVariable.Equals(Environments.Localhost)))
            // If no environment is set, the default production (appsettings.json) will be used
            hostBuilder.UseEnvironment(Environment.GetEnvironmentVariable(EnvironmentVariables.AppEnvironment));

        #endregion

        #region Serilog

        var logPath = Environment.GetEnvironmentVariable(EnvironmentVariables.AppSerilogFilePath);

        if (logPath is null || !logPath.EndsWith(".txt"))
            Environment.SetEnvironmentVariable(EnvironmentVariables.AppSerilogFilePath, Path.Combine(AppContext.BaseDirectory, "Logs", "log.txt"));

        #endregion

        return hostBuilder;
    }

    /// <summary>
    ///     Configurations all required appsettings for application for used environment
    /// </summary>
    public static IHostBuilder ConfigureAppSettings(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureAppConfiguration((context, builder) =>
        {
            builder
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();
        });
    }
}