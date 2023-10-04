using Application.Abstractions;
using Infrastructure.Clients.Cnb;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureDependencyInjection
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
        {

            serviceCollection.AddConfigurations(configuration);

            serviceCollection.AddClients(configuration);

            serviceCollection.AddRepositories();

            return serviceCollection;

        }

        private static IServiceCollection AddConfigurations(this IServiceCollection serviceCollection, IConfiguration configuration)
        {

            serviceCollection.Configure<ExchangeRateRepositoryConfiguration>(configuration.GetSection("CnbzRepositoryConfiguration"));

            return serviceCollection;

        }

        private static IServiceCollection AddClients(this IServiceCollection serviceCollection, IConfiguration configuration)
        {

            serviceCollection.AddHttpClient<ICnbzClient, CnbzClient>(client =>
            {
                CnbzConfiguration cnbzConfiguration = configuration.GetSection("Clients:Cnbz").Get<CnbzConfiguration>();
                client.BaseAddress = new Uri(cnbzConfiguration.Uri);
                client.Timeout = TimeSpan.FromMilliseconds(cnbzConfiguration.TimeoutMilliseconds);
            });

            return serviceCollection;

        }

        private static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
        {

            serviceCollection.AddTransient<IExchangeRateRepository, ExchangeRateRepository>();

            return serviceCollection;

        }

    }
}
