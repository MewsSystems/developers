using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Infrastructure.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Infrastructure;

public static class DependencyInjection
{
	public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddHttpClient<ICzechNationalBankExchangeRateClient, CzechNationalBankExchangeRateClient>((_, httpClient) =>
		{
			httpClient.BaseAddress = new Uri(configuration.GetSection("ApiUrls")["CzechNationalBankExRates"] ?? "");
		});
	}
}