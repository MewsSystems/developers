using CnbServiceClient.Interfaces;
using CnbServiceClient.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Utils.Extensions;

namespace CnbServiceClient.Extensions
{
	public static class ServiceCollectionExtension
	{
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

