using ExchangeRate.Client.Cnb.Extensions;
using ExchangeRate.Service.Abstract;
using ExchangeRate.Service.Factory;
using ExchangeRate.Service.Service;
using Framework.Caching.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRate.Service.Extensions
{
	public static class ConfigureServicesExtensions
	{
		public static void AddExchangeRateService(this IServiceCollection services, IConfiguration configuration)
		{
			//Add framework cache
			services.AddFrameworkCache();
			//Add Exchange rate cnb client
			services.AddExchangeRateClientCnbServices(configuration);
			//Register Service
			services.AddScoped<IExchangeRateServiceFactory, ExchangeRateServiceFactory>();
			services.AddScoped<IExchangeRateService, ExchangeRateService>();
		}
	}
}
