using ExchangeRates.Clients;
using ExchangeRates.Contracts;
using ExchangeRates.Parsers;
using ExchangeRates.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExchangeRates
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddCustomizedClients(this IServiceCollection services)
		{
			services.Scan(scan => scan
				.FromAssemblies(ExchangeRatesAssembly.Get())
					.AddClasses(c => c.AssignableTo(typeof(IClient<>)))
					.AsSelfWithInterfaces()
					.WithScopedLifetime());

			return services;
		}

		public static IServiceCollection AddCustomizedParsers(this IServiceCollection services)
		{
			services.Scan(scan => scan
				.FromAssemblies(ExchangeRatesAssembly.Get())
					.AddClasses(c => c.AssignableTo(typeof(IParser<>)))
					.AsSelfWithInterfaces()
					.WithScopedLifetime());

			return services;
		}

		public static IServiceCollection AddCustomizedExchangeRateProviders(this IServiceCollection services)
		{
			services.Scan(scan => scan
				.FromAssemblies(ExchangeRatesAssembly.Get())
					.AddClasses(c => c.AssignableTo(typeof(IExchangeRateProvider)))
					.AsSelfWithInterfaces()
					.WithScopedLifetime());

			return services;
		}

		public static IServiceCollection AddCustomizedCultureProviders(this IServiceCollection services)
		{
			services.Scan(scan => scan
				.FromAssemblies(ExchangeRatesAssembly.Get())
					.AddClasses(c => c.AssignableTo(typeof(ICultureProvider)))
					.AsSelfWithInterfaces()
					.WithSingletonLifetime());

			return services;
		}

		public static IServiceCollection AddCustomizedLogging(this IServiceCollection services)
		{
			services.AddLogging(configure => configure.AddConsole());

			return services;
		}

		public static IServiceCollection AddCustomizedOptions(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<AppSettings>(configuration);			
			return services;
		}
	}
}
