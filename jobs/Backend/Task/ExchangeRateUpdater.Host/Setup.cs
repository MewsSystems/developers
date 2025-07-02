using ExchangeRateUpdater.Core.Clients.CNB;
using ExchangeRateUpdater.Core.UseCases.Queries.GetExchangeRates;
using ExchangeRateUpdater.Host.Middleware;
using ExchangeRateUpdater.Logic.Clients.CNB;
using ExchangeRateUpdater.Logic.UseCases.Queries.GetExchangeRates;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ExchangeRateUpdater.Host
{
	public static class Setup
	{
		public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddLogging();
			serviceCollection.AddMemoryCache();

			serviceCollection.AddHttpClient<ICzechNationalBankService, CzechNationalBankService>()
			.ConfigureHttpClient(httpClient =>
			{
				httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("BANK_CLIENT_URL"));
			});

			//Register queries
			serviceCollection.AddTransient<IGetExchangeRateQuery, GetExchangeRateQuery>();

			return serviceCollection;
		}

		public static IApplicationBuilder RegisterMiddlewares(this IApplicationBuilder appBuilder)
		{
			appBuilder.UseMiddleware<ErrorHandlingMiddleware>();

			return appBuilder;
		}
	}
}
