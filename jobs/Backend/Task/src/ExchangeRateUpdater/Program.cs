using ExchangeRateUpdater.Contracts;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRateUpdater
{
	public static class Program
	{
		private static IEnumerable<Currency> currencies = new[]
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

		public static async Task Main(string[] args)
		{
			using IHost host = Host.CreateDefaultBuilder(args)
				.ConfigureServices((context, services) =>
				{
					var configuration = context.Configuration;
					services.AddOptions<CnbDailyRatesOptions>()
						.Bind(configuration.GetSection(CnbDailyRatesOptions.SectionName))
						.ValidateDataAnnotations();
					services.AddHttpClient();
					services.AddSingleton<ICnbClient, CnbClient>();
					services.AddSingleton<ICnbParser, CnbParser>();
					services.AddSingleton<ICnbFxRateProvider, CnbFxRateProvider>();
					services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
				})
				.Build();

            try
			{
				var provider = host.Services.GetRequiredService<IExchangeRateProvider>();
				var rates = await provider.GetExchangeRatesAsync(currencies);

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
		}
	}
}