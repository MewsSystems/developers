using ExchangeRateUpdater.Application.DependencyInjection;
using ExchangeRateUpdater.Application.Settings;
using ExchangeRateUpdater.ConsoleApp.Services;
using ExchangeRateUpdater.Infrastructure.Configuration;
using ExchangeRateUpdater.Infrastructure.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

var host = Host
    .CreateDefaultBuilder()
    .ConfigureAppConfiguration((context, config) =>
    {
        var basePath = AppContext.BaseDirectory;
        config.SetBasePath(basePath);
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        config.AddEnvironmentVariables();
    })
    .UseSerilog((context, config) =>
    {
        config.ReadFrom.Configuration(context.Configuration);
    })
    .ConfigureServices((context, services) =>
    {
        IConfiguration configuration = context.Configuration;

        // Register Configuration Settings
        services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));
        services.Configure<ApiSettings>(configuration.GetSection("ApiSettings"));
        services.Configure<HttpClientSettings>(configuration.GetSection("HttpClientSettings"));

        // Register Application and Infrastructure layers
        services.AddApplication();
        services.AddInfrastructure(configuration.GetSection(nameof(HttpClientSettings)).Get<HttpClientSettings>() ?? new());

        // Register Console Service
        services.AddSingleton<ExchangeRateConsoleService>();

        // Add MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
    })
    .Build();

// Execute the application
using var scope = host.Services.CreateScope();
var service = scope.ServiceProvider.GetRequiredService<ExchangeRateConsoleService>();
await service.RunAsync();
