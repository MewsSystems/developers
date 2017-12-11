namespace ExchangeRateUpdater.Core.Unity.Extensions {
	using System.Collections.Generic;
	using System.Linq;
	using ExchangeRateUpdater.Diagnostics;
	using global::Unity;
	using global::Unity.Registration;

	public static class IUnityContainerExtensions {
		private static IEnumerable<IContainerRegistration> GetRegistrations(this IUnityContainer container) {
			var result = Ensure.IsNotNull(container?.Registrations);

			return result;
		}

		public static IEnumerable<IContainerRegistration> GetRegistrationsForType<T>(this IUnityContainer container, bool onlyNamed = false) {
			var result = container
				.GetRegistrations()
				.Where(r => r.RegisteredType == typeof(T) && (!Check.IsNullOrWhiteSpace(r.Name) || !onlyNamed));

			return result;
		}
	}
}
