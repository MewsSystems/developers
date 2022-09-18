using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;
using Ninject;

[assembly: InternalsVisibleTo("ExchangeRateUpdaterTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace ExchangeRateUpdater
{
	internal class Program
	{
		private static readonly IEnumerable<Currency> currencies = new[]
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
		private readonly IExchangeRateProvider exchangeRateProvider;
		private readonly ILogger logger;

		public Program(IExchangeRateProvider exchangeRateProvider, ILogger logger)
		{
			this.exchangeRateProvider = exchangeRateProvider;
			this.logger = logger;
		}

		public static async Task Main()
		{
			var kernel = new StandardKernel(new DependencyInjectionConfiguration());
			var program = kernel.Get<Program>();

			await program.RunAsync().ConfigureAwait(false);

			Console.ReadLine();
		}

		public async Task RunAsync()
		{
			try
			{
				var rates = (await exchangeRateProvider.GetExchangeRatesAsync(currencies).ConfigureAwait(false)).ToList();

				await logger.LogInfoAsync($"Successfully retrieved {rates.Count} exchange rates:").ConfigureAwait(false);

				foreach (var rate in rates)
				{
					await logger.LogInfoAsync(rate.ToString()).ConfigureAwait(false);
				}
			}
			catch (Exception e)
			{
				await logger.LogErrorAsync($"Could not retrieve exchange rates: '{e.Message}'.").ConfigureAwait(false);
			}
		}
	}
}
