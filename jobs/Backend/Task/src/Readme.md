# Mews backend task - Exchange Rate Updater solution
This is a repository with solution for Mews backend task. Said task is supposed to fetch exchange rates from Czech National banks for their national currency. 
## Requirements
For sucessfull running of the solution certain packages are required:
* Serilog - consists of several packages such as Serilog sinks for different kinds of outputs (console and file in this solution), Serilog hosting and configuration for configuring logging via appsettings json file
* Microsoft.Extensions - extensions package that consists of different package needed for setting up of the program.cs class:
    * Microsoft.Extensions.Caching.Memory - package for in memory caching of returned exchange rates
    * Microsoft.Extensions.Configuration - package for binding appsettings json to a custom settings class
    * Microsoft.Extensions.Logging - package for logger used in Serilog
    * Microsoft.Extensions.Http - package for http client for sending get requests to the bank API
## Logging
Logging in this solution is done via Serilog which allows the user to setup a logger with just few lines of code. It allows the user to have logging done directly to different outpouts such as:
* Message broker
* Console
* File

```
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();
Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .CreateLogger();
var host = new HostBuilder().UseSerilog();
```
## How To Run
Use dotnet run to run the solution
## Projects
### [App](ExchangeRateUpdater.App)

### [Domain](ExchangeRateUpdater.Domain)
### [Tests](ExchangeRateUpdater.Tests)

## Configuration
Configuration is stored in [appsettings.json](ExchangeRateUpdater.App/appsettings.json) file which is parsed inside [settings](ExchangeRateUpdater.Domain/Configurations/ExchangeRateProviderSettings.cs) file. It allows the user to enter the API endpoint where exchange rates can be found. 

## Testing

Running dotnet test inside [Tests](ExchangeRateUpdater.Tests/) directory will execute the application tests.


