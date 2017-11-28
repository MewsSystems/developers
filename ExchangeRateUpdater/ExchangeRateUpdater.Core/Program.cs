namespace ExchangeRateUpdater.Core {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using ExchangeRateUpdater.Core.Strings;
	using ExchangeRateUpdater.Core.Unity.Extensions;
	using ExchangeRateUpdater.Financial;
	using ExchangeRateUpdater.Unity;
	using global::Unity;

	public static class Program {
		private static IEnumerable<Currency> currencies = new[] {
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

		public static async Task Main(string[] args) {
			Restart:
			try {
				using (var context = new ProgramContext(new UnityConfiguration().Configure(new UnityContainer()))) {
					var providerKeys = context.Container.GetProviderKeys();

					SelectProvider:
					string selectedIndex = SelectExchangeRateProvider(providerKeys);

					if (!Int32.TryParse(selectedIndex, out int index) || (index < 0 && index >= providerKeys.Length)) {
						WriteInvalidInput();
						goto SelectProvider;
					}

					WriteLoadingMessage();

					var provider = context.Container.Resolve<IExchangeRateProvider>(providerKeys[index]);

					var exchangeRates = await provider.GetExchangeRatesAsync(currencies);

					WriteExchangeRates(exchangeRates);

					SelectStartOver:
					var startOver = SelectStartOver();

					if (startOver.Key == ConsoleKey.Y) {
						Console.Clear();
						goto SelectProvider;
					} else if (startOver.Key != ConsoleKey.N) {
						WriteInvalidInput();
						goto SelectStartOver;
					}
				}
			} catch (Exception e) {
				Console.WriteLine(String.Format(ConsoleMessageResource.GenericErrorMessageFormat, e.Message));
				goto Restart;
			}
		}

		private static void WriteExchangeRates(IEnumerable<ExchangeRate> exchangeRates) {
			Console.Clear();
			Console.WriteLine(String.Format(ConsoleMessageResource.SuccesfullyRetrivedExchangeRatesMessageFormat, exchangeRates.Count()));
			Console.WriteLine();
			foreach (var rate in exchangeRates) {
				Console.WriteLine(rate.ToString());
			}
			Console.WriteLine();
		}

		private static void WriteLoadingMessage() {
			Console.WriteLine();
			Console.WriteLine(ConsoleMessageResource.GenericLoadingMessage);
			Console.WriteLine();
		}

		private static ConsoleKeyInfo SelectStartOver() {
			Console.WriteLine();
			Console.WriteLine(ConsoleMessageResource.StartOverQuestionMessage);
			Console.WriteLine();

			var result = Console.ReadKey();

			return result;
		}

		private static string SelectExchangeRateProvider(string[] providerKeys) {
			Console.WriteLine();
			Console.WriteLine(ConsoleMessageResource.SelectExchangeRateProviderConsoleMessage);
			Console.WriteLine();

			for (int i = 0; i < providerKeys.Length; i++) {
				Console.WriteLine($"{i}: {providerKeys[i]}");
			}

			Console.WriteLine();

			return Console.ReadLine().Trim();
		}

		private static void WriteInvalidInput() {
			Console.WriteLine();
			Console.WriteLine(ConsoleMessageResource.InvalidInputConsoleMessage);
			Console.WriteLine();
		}
	}
}
