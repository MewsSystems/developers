namespace ExchangeRateUpdater.Core {
	using System;
	using System.Threading.Tasks;
	using ExchangeRateUpdater.Core.Strings;
	using ExchangeRateUpdater.Unity;
	using global::Unity;

	public static class Program {
		public static async Task Main(string[] args) {
			try {
				using (var context = new ProgramContext(new UnityConfiguration().Configure(new UnityContainer()))) {
					do {
						if (!context.IsInitialized) {
							context.Initialize();
						}

						var result = await context.RunAsync();

						if (result) {
							context.Next();
						}

						Console.Clear();
					} while (context.Continue);
				}
			} catch (Exception e) {
				Console.WriteLine(String.Format(ConsoleMessageResource.GenericErrorMessageFormat, e.Message));
			}
		}
	}
}
