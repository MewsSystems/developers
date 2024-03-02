using CNB.Client;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mime;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class CnbClientServiceCollectionExtension
{
    public static IServiceCollection AddCnbClient(this IServiceCollection services)
    {
        services.Configure<CnbClientOptions>(options =>
        {
            options.BaseUrl = new Uri("https://api.cnb.cz");
        });

        services.AddHttpClient<IBankClient, CnbClient>()
                         .ConfigureHttpClient((provider, client) =>
                         {
                             var options = provider.GetRequiredService<IOptions<CnbClientOptions>>();

                             client.BaseAddress = options.Value.BaseUrl;
                             client.DefaultRequestHeaders.Add("Accept", MediaTypeNames.Application.Json);
                         })
                         .ConfigurePrimaryHttpMessageHandler(handler => new HttpClientHandler()
                         {
                             AutomaticDecompression = DecompressionMethods.Brotli | DecompressionMethods.GZip | DecompressionMethods.None
                         });

        return services;
    }
}

