using ExchangeRateUpdater.Core.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Core.IoC;

public static class IoCExtensions
{
	public static IServiceCollection RegisterCore(this IServiceCollection services) =>
		services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
}