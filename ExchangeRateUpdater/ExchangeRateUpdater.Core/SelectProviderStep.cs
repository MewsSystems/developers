namespace ExchangeRateUpdater.Core {
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using ExchangeRateUpdater.Core.Strings;
	using ExchangeRateUpdater.Core.Unity.Extensions;
	using ExchangeRateUpdater.Financial;

	public class SelectProviderStep : ProgramStep {
		public override async Task<bool> RunAsync(ProgramContext context) {
			var providers = context.Container.GetRegistrationsForType<IExchangeRateProvider>(true).ToList();

			context.WriteMessage(ConsoleMessageResource.SelectExchangeRateProviderConsoleMessage);
			context.WriteMessageRange(providers.Select((p, i) => $"{i}: {p.Name}"));

			var key = context.ReadKey();
			Console.WriteLine();

			var result = context.ValidateInput(key, providers.Select((p, i) => Enum.Parse<ConsoleKey>($"D{i}")));

			if (result) {
				context.TempData.SelectedProvider = providers.ElementAt(Int32.Parse(key.ToString().Substring(1, 1)))?.Name;
			}

			return result;
		}
	}
}
