namespace ExchangeRateUpdater.Core.Unity.Extensions {
	using System.Linq;
	using ExchangeRateUpdater.Diagnostics;
	using ExchangeRateUpdater.Financial;
	using global::Unity;

	public static class IUnityContainerExtensions {
		public static string[] GetProviderKeys(this IUnityContainer container) {
			var registrations = Ensure.IsNotNull(container?.Registrations);

			var result = registrations
				.Where(r => r.RegisteredType == typeof(IExchangeRateProvider) && !Check.IsNullOrWhiteSpace(r.Name))
				.Select(r => r.Name)
				.ToArray();

			return result;
		}

		
	}
}
