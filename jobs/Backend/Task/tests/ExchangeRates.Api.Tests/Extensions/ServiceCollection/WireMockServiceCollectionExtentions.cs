using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WireMock.Server;

namespace ExchangeRates.Api.Tests.Extensions.ServiceCollection;

public static class WireMockServiceCollectionExtentions
{
    public static IServiceCollection ConfigureWireMock(this IServiceCollection services, IConfiguration configuration)
    {
        var cnbBaseUrl = configuration["HttpClients:Cnb:BaseUrl"]!;

        var cnbWireMockServer = WireMockServer.Start(cnbBaseUrl);

        return services.AddSingleton(cnbWireMockServer);
    }
}
