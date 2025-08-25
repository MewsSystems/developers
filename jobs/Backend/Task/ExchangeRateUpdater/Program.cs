using ExchangeRateProviders;
using ExchangeRateProviders.Core;
using ExchangeRateProviders.Core.Model;
using ExchangeRateProviders.Czk;
using ExchangeRateProviders.Czk.Clients;
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
    new("TRY"),
    new("XYZ")
};

var builder = Host.CreateApplicationBuilder(args);
ConfigureServices(builder.Services);

using var host = builder.Build();

try
{
    var exchangeRateService = host.Services.GetRequiredService<IExchangeRateService>();
    var rates = await exchangeRateService.GetExchangeRatesAsync(ExchangeRateProviderTargetCurrencyCode, currencies, CancellationToken.None);

	Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
    foreach (var rate in rates)
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
    services.AddFusionCache();

	//Register HtpClients for API clients
	services.AddHttpClient<ICzkCnbApiClient, CzkCnbApiClient>();

	//Register exchange rate providers and factory
	services.AddSingleton<IExchangeRateDataProvider, CzkExchangeRateDataProvider>();
	services.AddSingleton<IExchangeRateDataProviderFactory, ExchangeRateDataProviderFactory>();

	//Register the exchange rate service
	services.AddSingleton<IExchangeRateService, ExchangeRateService>();
}