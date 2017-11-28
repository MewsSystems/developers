namespace ExchangeRateUpdater {
	using Unity;
	using OpenExchangeRates;
	using OpenExchangeRates.Http;
	using ExchangeRateUpdater.Financial;
	using System.Linq;

	public static class UnityConfig {
		public static readonly UnityContainer Container = new UnityContainer();

		public static void Configure() {
			Container.RegisterType<IOpenExchangeRateClient, OpenExchangeRateClient>();
			Container.RegisterType<IExchangeRateProvider, OpenExchangeRateProvider>("USD");
		}
	}
}
