using CnbServiceClient.Interfaces;
using CnbServiceClient.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CnbServiceClient.Extensions
{
	public static class ServiceCollectionExtension
	{
        public static IServiceCollection AddCnbServiceClient(this IServiceCollection services, IConfiguration configuration)
        {
            string url = GetServiceClientUrl(configuration);

            services.AddHttpClient<IExratesService, ExratesService>(client =>
            {
                client.BaseAddress = new Uri(url);
            });

            return services;
        }

        private static string GetServiceClientUrl(IConfiguration configuration)
        {
            var url = configuration["ServiceClient:Url"];

            if (string.IsNullOrEmpty(url))
            {
                throw new InvalidOperationException("The service client url is missing in the configuration. Please add it as `ServiceClient:Url`.");
            }

            return url;
        }
    }
}

