using ExchangeRateUpdater;
using ExchangeRateUpdater.Service;
using Microsoft.Extensions.DependencyInjection;

IEnumerable<Currency> currencies = new[]
{
	new Currency("USD"),
	new Currency("EUR"),
	new Currency("CZK"),
	new Currency("JPY"),
	new Currency("KES"),
	new Currency("RUB"),
	new Currency("THB"),
	new Currency("TRY"),
	new Currency("XYZ")
};

var serviceCollection = new ServiceCollection();
ConfigureServices(serviceCollection);
var serviceProvider = serviceCollection.BuildServiceProvider();

var provider = serviceProvider.GetRequiredService<ExchangeRateProvider>();

try
{
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

Console.ReadLine();

static void ConfigureServices(ServiceCollection serviceCollection)
{
	serviceCollection.AddHttpClient("csk_bank_ex_rates_daily", c => c.BaseAddress = new Uri($"https://api.cnb.cz/cnbapi/exrates/daily?date={DateTime.UtcNow.ToString("yyyy-MM-dd")}&lang=EN"));

	serviceCollection.AddSingleton<ExchangeRateProvider>();
	serviceCollection.AddSingleton<ICentralBankService, CentralBankService>();
}