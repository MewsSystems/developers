namespace ExchangeRateUpdater.Core {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using ExchangeRateUpdater.Core.Strings;
	using ExchangeRateUpdater.Diagnostics;
	using ExchangeRateUpdater.Financial;
	using global::Unity;

	public class GetExchangeRatesStep : ProgramStep {
		public override async Task<bool> RunAsync(ProgramContext context) {
			context.WriteMessage(ConsoleMessageResource.GenericLoadingMessage);
			var providerName = (string)Ensure.IsNotNullOrWhiteSpace(context.TempData.SelectedProvider, nameof(context.TempData.SelectedProvider));

			var provider = context.Container.Resolve<IExchangeRateProvider>(providerName);

			var exchangeRates = await provider.GetExchangeRatesAsync(context.Container.Resolve<IEnumerable<Currency>>());

			Console.Clear();

			context.WriteMessage(String.Format(ConsoleMessageResource.SuccesfullyRetrivedExchangeRatesMessageFormat, exchangeRates.Count()));
			context.WriteMessageRange(exchangeRates.Select(er => er.ToString()));

			context.ReadKey();
			Console.WriteLine();

			return true;
		}
	}
}
