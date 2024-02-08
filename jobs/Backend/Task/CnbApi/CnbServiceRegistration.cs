using CnbApi.Client;
using ExchangeRateUpdater.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CnbApi;
public interface IServiceRegistration
{
    void RegisterServices(IServiceCollection services);
}

public class CnbServiceRegistration : IServiceRegistration
{
    public void RegisterServices(IServiceCollection services)
    {
        var config = new ConfigurationBuilder()
        .AddJsonFile("appSettings.json")
        .Build();

        services
            .AddHttpClient<ICnbClient, CnbClient>();

        // Make config injectable
        services.Configure<ApiSettings>(config.GetSection("apiSettings"));
    }
}

