using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.Application.ExchangeRates.Query.GetExchangeRatesDaily;
using ExchangeRateUpdater.Infrastructure;
using Mediator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

string GetEnvironment()
    => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environments.Production;

void ConfigureConfigurationBuilder(IConfigurationBuilder config, string[] args, string environment)
    => config
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false)
        .AddJsonFile($"appsettings.{environment.ToLower()}.json", optional: true)
        .AddEnvironmentVariables();

var host = new HostBuilder()
    .UseEnvironment(GetEnvironment())
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        ConfigureConfigurationBuilder(config, args, hostingContext.HostingEnvironment.EnvironmentName);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddApplicationServices()
            .AddInfrastructure(hostContext.Configuration);
    })
    .Build();


try
{
    var mediator = host.Services.GetRequiredService<IMediator>();
    var response = await mediator.Send(new GetExchangesRatesByDateQuery
    {
        CurrencyCodes = new List<string>()
        {
            "USD", "EUR", "CZK", "JPY",
            "KES", "RUB", "THB", "TRY", "XYZ"
        },
        Date = null,
        Language = null
    });


    Console.WriteLine($"Successfully retrieved {response.Count} exchange rates:");
    foreach (var rate in response)
    {
        Console.WriteLine(rate.ToString());
    }
}
catch (Exception e)
{
    Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
}

Console.ReadLine();