using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater.Console.Configuration;

public class Settings
{
    public string ExchangeRateBaseUrl { get; set; }
    public string ExchangeRateBaseCurrency { get; set; }
    public string TimezoneId { get; set; }
    public MappingSettings MappingSettings { get; set; }

    public static Settings From(IConfiguration configuration)
    {
        var settings = new Settings();

        var appConfiguration = configuration.GetSection(Program.ApplicationName);

        appConfiguration.Bind(settings);

        return settings;
    }
}