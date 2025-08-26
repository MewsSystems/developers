using ExchangeRateProviders.Core;
using Microsoft.Extensions.DependencyInjection;
using ExchangeRateProviders.Czk;
using ExchangeRateProviders.Czk.Clients;
using ExchangeRateProviders.Usd;

namespace ExchangeRateProviders;

public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Registers ExchangeRateProviders core services, data providers and dependencies.
	/// </summary>
	/// <param name="services">The service collection.</param>
	/// <returns>The same service collection for chaining.</returns>
	public static IServiceCollection AddExchangeRateProviders(this IServiceCollection services)
	{
		// FusionCache (caller can configure options separately if desired)
		services.AddFusionCache();

		// HTTP clients for external APIs (add more as new providers are implemented)
		services.AddHttpClient<ICzkCnbApiClient, CzkCnbApiClient>();

		// Data providers (add any additional provider registrations here)
		services.AddSingleton<IExchangeRateDataProvider, CzkExchangeRateDataProvider>();
		services.AddSingleton<IExchangeRateDataProvider, UsdExchangeRateDataProvider>();

		// Factory & service
		services.AddSingleton<IExchangeRateDataProviderFactory, ExchangeRateDataProviderFactory>();
		services.AddSingleton<IExchangeRateService, ExchangeRateService>();

		return services;
	}
}
