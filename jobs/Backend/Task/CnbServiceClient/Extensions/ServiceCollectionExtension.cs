using CnbServiceClient.Interfaces;
using CnbServiceClient.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Utils.Extensions;

namespace CnbServiceClient.Extensions
{
	public static class ServiceCollectionExtension
	{
        /// <summary>
        /// Adds all the CnbServiceClient services and configurations.
        /// </summary>
        /// <param name="services">Service Collection.</param>
        /// <param name="configuration">Configuration.</param>
        /// <returns><see cref="IServiceCollection"/> with all the services needed by the project.</returns>
        public static IServiceCollection AddCnbServiceClient(this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration.GetRequiredValue<string>("ServiceClient:Url");

            services.AddHttpClient<IExratesService, ExratesService>(client =>
            {
                client.BaseAddress = new Uri(url);
            });

            return services;
        }
    }
}

