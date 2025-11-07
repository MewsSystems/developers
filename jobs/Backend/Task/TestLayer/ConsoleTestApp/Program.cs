using ConsoleTestApp.Config;
using ConsoleTestApp.Entertainment;
using ConsoleTestApp.UI;
using Microsoft.Extensions.Configuration;

// Load configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var appSettings = new AppSettings();
configuration.Bind(appSettings);

// Show startup entertainment (if enabled)
if (appSettings.Entertainment.Enabled)
{
    var entertainment = new StartupEntertainment(appSettings.Entertainment);
    await entertainment.ShowAsync();
}

// Start interactive console
var console = new InteractiveConsole(appSettings);
await console.RunAsync();
