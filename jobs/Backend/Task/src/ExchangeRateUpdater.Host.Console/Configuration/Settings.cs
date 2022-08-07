using System.IO;
using Microsoft.Extensions.Configuration;
using Serilog.Events;

namespace ExchangeRateUpdater.Host.Console.Configuration;

internal class Settings : ISettings
{
    public string HttpLogEndpoint { get; set; }
    public LogEventLevel MinimumLogLevel { get; set; }
    
    public static IConfiguration GetSettingsConfiguration()
    {
        IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
        
        return builder.Build();
    }
    
    public static Settings From(IConfiguration configuration, string applicationName)
    {
        var settings = new Settings();

        var appConfiguration = configuration
            .GetSection(applicationName);
            
        appConfiguration.Bind(settings);

        return settings;
    }
}