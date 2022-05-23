using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Framework.Logging.Extensions
{
	public static class ConfigureServicesExtensions
	{
		public static void AddFrameworkLogging(this IServiceCollection services, IConfiguration configuration)
		{
			// Add logging part
			services.AddLogging(logging =>
			{
				logging.ClearProviders();
				logging.AddConfiguration(configuration.GetSection("Logging"));
				logging.AddConsole();
			});
		}
	}
}
