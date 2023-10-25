using ExchangeRateUpdater.Core.Adapters;
using ExchangeRateUpdater.Core.Adapters.CzechNationalBankApi;
using ExchangeRateUpdater.Core.Adapters.CzechNationalBankApi.Configurations;
using ExchangeRateUpdater.Core.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace ExchangeRateUpdater.Core.IoC;

public static class IoCExtensions
{
	public static HostApplicationBuilder RegisterCore(this HostApplicationBuilder host,
		IConfigurationRoot configurationRoot)
	{
		host.Services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
		RegisterExchangeRateApi();


		return host;

		void RegisterExchangeRateApi()
		{
			var czechNationalBankApiConfigSection = configurationRoot.GetSection(nameof(CzechNationalBankApiConfig));

			//TODO: If multiple apis available, fail if none is configured.
			if (!czechNationalBankApiConfigSection.Exists())
			{
				throw new ArgumentNullException($"Missing {nameof(CzechNationalBankApiConfig)} on appsettings.json");
			}


			host.Services
				.AddSingleton(czechNationalBankApiConfigSection.Get<CzechNationalBankApiConfig>()!);

			host.Services.AddHttpClient<IExchangeRateApiAdapter,CzechNationalBankApiAdapter>((services, client) =>
			{
				client.BaseAddress = new Uri(services.GetRequiredService<CzechNationalBankApiConfig>().BaseUrl);
			});
		}
	}
}