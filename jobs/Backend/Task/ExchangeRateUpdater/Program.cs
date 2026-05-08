using ExchangeRateUpdater.Core.Configuration;
using ExchangeRateUpdater.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        if (context.HostingEnvironment.IsDevelopment())
        {
            config.AddEnvironmentVariables()
                .AddJsonFile("appsettings.json")
            .AddUserSecrets<Program>();
        }
    })
    .ConfigureServices((context, services) =>
    {
        services.AddLogging();
        services.AddCore(context.Configuration);
        services.AddInfrastructure(context.Configuration);
        services.AddHostedService<ExchangeRateUpdater.Startup.ExchangeRateStartupService>();
        
    })
    .Build();

await host.RunAsync();