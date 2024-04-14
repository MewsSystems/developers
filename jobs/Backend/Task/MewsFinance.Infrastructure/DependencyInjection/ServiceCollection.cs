using MewsFinance.Application.Clients;
using MewsFinance.Infrastructure.CnbFinancialClient.Mappings;
using MewsFinance.Infrastructure.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace MewsFinance.Infrastructure.DependencyInjection
{
    public static class ServiceCollection
    {
        public static void AddInfrastructure(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpClient<IHttpClientWrapper, HttpClientWrapper>()
            .ConfigureHttpClient(httpClient =>
            {
                    httpClient.BaseAddress = new Uri("https://api.cnb.cz/cnbapi/");
                    httpClient.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
            });
            serviceCollection.AddScoped<IFinancialClient, CnbFinancialClient.CnbFinancialClient>();
            serviceCollection.AddAutoMapper(typeof(CnbFinancialClientProfile));
        }
    }
}
