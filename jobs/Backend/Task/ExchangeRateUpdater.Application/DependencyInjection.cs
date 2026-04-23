using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Application;

public static class DependencyInjection
{
	public static void AddApplication(this IServiceCollection services)
	{
		services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
	}
}