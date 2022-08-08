using System.IO;
using Domain.Ports;
using Microsoft.Extensions.Configuration;
using Serilog.Events;

namespace ExchangeRateUpdater.Host.Console.Configuration;

#nullable disable

internal class Settings : ISettings
{
    public string HttpLogEndpoint { get; set; }
    public LogEventLevel MinimumLogLevel { get; set; }
    public bool UseInMemoryCache { get; set; }
    public CzechNationalBankApiSettings CzechNationalBankApiSettings { get; set; }
    
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

internal class CzechNationalBankApiSettings : IExchangeRateApiSettings
{
    public string ApiBaseAddress { get; set; }
    public string DefaultExchangeRateTargetCurrency { get; set; }
    public string Delimiter { get; set; }
    public string DecimalSeparator { get; set; }
}

#nullable restore