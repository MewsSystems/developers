using ExchangeRateUpdater.Domain.Core.Clients;
using ExchangeRateUpdater.Domain.Core.UseCases.Queries.GetExchangeRates;
using ExchangeRateUpdater.Domain.Logic.Clients;
using ExchangeRateUpdater.Domain.Logic.UseCases.Queries.GetExchangeRates;
using ExchangeRateUpdater.Host.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ExchangeRateUpdater.Host
{
	public static class Setup
	{
		public static IServiceCollection RegisterServices(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddHttpClient<IHttpBankClientWrapper, HttpBankClientWrapper>()
			.ConfigureHttpClient(httpClient =>
			{
				httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("BANK_CLIENT_URL"));
			});

			serviceCollection.AddScoped<IBankClient, CzechNationalBankClient>();
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
