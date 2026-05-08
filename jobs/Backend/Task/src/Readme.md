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
Main part of the project. Inside of it are [service](ExchangeRateUpdater.App/Services/ExchangeRateService.cs) and [provider](ExchangeRateUpdater.App/Providers/ExchangeRateProvider.cs) alongside their interfaces for the fetching of exchange rates.

### [Domain](ExchangeRateUpdater.Domain)
Consists of all supporting classes ExchangeRateUpdater.App requires such as configuration, DTO, model, etc classes.
### [Tests](ExchangeRateUpdater.Tests)
Consits of tests for ExchangeRateUpdater.App service and provider.

## Configuration
Configuration is stored in [appsettings.json](ExchangeRateUpdater.App/appsettings.json) file which is parsed inside [settings](ExchangeRateUpdater.Domain/Configurations/ExchangeRateProviderSettings.cs) file. It allows the user to enter the API endpoint where exchange rates can be found. 

## Testing

Running dotnet test inside [Tests](ExchangeRateUpdater.Tests/) directory will execute the application tests.

## Possible improvements
* Caching - Caching could be implemented in Redis instead of in memory caching
* Docker, CI/CD - Was not required in the task but it is a must have thing in every modern production environment
* Performance tracing and metrics - Enterprise solutions require tracing of how solution is performing, Grafana could be used for creating easy to read dashboards for tracking. 


