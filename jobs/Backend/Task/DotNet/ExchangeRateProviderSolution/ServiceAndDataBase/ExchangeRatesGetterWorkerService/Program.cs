using ExchangeRatesGetterWorkerService;
using ExchangeRatesGetterWorkerService.Context;
using Microsoft.EntityFrameworkCore;


var builder = Host.CreateDefaultBuilder(args);

IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();

builder.ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default")));
       
    });

var host = builder.Build();

await host.RunAsync();
