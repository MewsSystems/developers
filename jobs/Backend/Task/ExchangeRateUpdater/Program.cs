using ExchangeRateProviders;
using ExchangeRateProviders.Core;
using ExchangeRateProviders.Core.Model;
using ExchangeRateProviders.Czk;
using ExchangeRateProviders.Czk.Clients;
using ExchangeRateProviders.Czk.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

const string ExchangeRateProviderCurrencyCode = "CZK";

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
    var factory = host.Services.GetRequiredService<IExchangeRateProviderFactory>();
    var provider = factory.GetProvider(ExchangeRateProviderCurrencyCode);
    var rates = await provider.GetExchangeRatesAsync(currencies, CancellationToken.None);

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
    services.AddHttpClient<ICzkCnbApiClient, CzkCnbApiClient>();
    services.AddTransient<IExchangeRateDataProvider, CzkExchangeRateDataProviderSevice>();
    services.AddSingleton<IExchangeRateProvider, CzkExchangeRateProvider>();
    services.AddSingleton<IExchangeRateProviderFactory, ExchangeRateProviderFactory>();
}