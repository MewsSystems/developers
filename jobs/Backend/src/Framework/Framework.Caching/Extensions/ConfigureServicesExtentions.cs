using Framework.Caching.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Caching.Extensions
{
	public static class ConfigureServicesExtensions
	{
		public static void AddFrameworkCache(this IServiceCollection services)
		{
			services.AddMemoryCache();
			services.AddSingleton<ICache, Cache>();
		}
	}
}
