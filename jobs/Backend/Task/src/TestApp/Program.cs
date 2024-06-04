using ExchangeRateUpdater;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


IEnumerable<Currency> currencies =
[
	new Currency("USD"),
    new Currency("EUR"),
    new Currency("CZK"),
    new Currency("JPY"),
    new Currency("KES"),
    new Currency("RUB"),
    new Currency("THB"),
    new Currency("TRY"),
    new Currency("XYZ")
];


var configBuilder = new ConfigurationBuilder();
configBuilder.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHttpClient<IExchangeRateProvider, ExchangeRateProvider>(client =>
        {
            var baseUrl = context.Configuration["BaseUrl"] ?? throw new Exception("BaseUrl configuration not found.");

			client.BaseAddress = new Uri(baseUrl);
        });
    });

var host = builder.Build();


try
{
    var provider = host.Services.GetRequiredService<IExchangeRateProvider>();
    var rates = await provider.GetExchangeRates(currencies);

    Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
    foreach (var rate in rates)
    {
        Console.WriteLine(rate.ToString());
    }
}
catch (Exception e)
{
    Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
}

Console.WriteLine("Press any key to close the app.");
Console.ReadLine();
