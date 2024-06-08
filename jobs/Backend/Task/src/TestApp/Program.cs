using ExchangeRateProvider.BankApiClients.Cnb;
using ExchangeRateProvider.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;

var configBuilder = new ConfigurationBuilder();
configBuilder.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
	    services
		    .AddLogging(builder => builder.AddConsole())
		    .AddMemoryCache(options =>
		    {
			    options.SizeLimit = 1024;
		    })
		    .AddSingleton<IExchangeRateProvider, ExchangeRateProvider.ExchangeRateProvider>()
		    .AddHttpClient<IBankApiClient, CnbBankApiClient>(client =>
		    {
			    var baseUrl = context.Configuration["BaseUrl"] ??
			                  throw new Exception("BaseUrl configuration not found.");

			    client.BaseAddress = new Uri(baseUrl);
		    })
		    .AddStandardResilienceHandler(options =>
		    {
			    options.Retry.MaxRetryAttempts = 4;
			    options.Retry.BackoffType = DelayBackoffType.Exponential;
				options.Retry.UseJitter = true;
		    });
    });

var host = builder.Build();


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

var cancellationToken = new CancellationTokenSource();

try
{
    var provider = host.Services.GetRequiredService<IExchangeRateProvider>();
    var rates = await provider.GetExchangeRatesAsync(currencies, null, cancellationToken.Token).ConfigureAwait(false);

    var exchangeRates = rates as ExchangeRate[] ?? rates.ToArray();
    Console.WriteLine($"Successfully retrieved {exchangeRates.Length} exchange rates:");
    foreach (var rate in exchangeRates)
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
