using ExchangeRateProviders;
using ExchangeRateProviders.Core;
using ExchangeRateProviders.Core.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

const string ExchangeRateProviderTargetCurrencyCode = "CZK";

var currencies = new List<Currency>
{
    new("USD"),
    new("EUR"),
    new("CZK"),
    new("JPY"),
    new("KES"),
    new("RUB"),
    new("THB"),
    new("TRY")
};

var builder = Host.CreateApplicationBuilder(args);
ConfigureServices(builder.Services);

using var host = builder.Build();

try
{
    var exchangeRateService = host.Services.GetRequiredService<IExchangeRateService>();
    var czkrates = await exchangeRateService.GetExchangeRatesAsync(ExchangeRateProviderTargetCurrencyCode, currencies, CancellationToken.None);

	Console.WriteLine($"Successfully retrieved {czkrates.Count()} exchange rates:");
    foreach (var rate in czkrates)
    {
        Console.WriteLine(rate.ToString());
    }

    var usdrates = await exchangeRateService.GetExchangeRatesAsync("USD", currencies, CancellationToken.None);
	Console.WriteLine($"Successfully retrieved {czkrates.Count()} exchange rates:");
	foreach (var rate in usdrates)
	{
		Console.WriteLine(rate.ToString());
	}
}
catch (Exception e)
{
    Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'");
}

return;

static void ConfigureServices(IServiceCollection services)
{
    services.AddExchangeRateProviders();
}