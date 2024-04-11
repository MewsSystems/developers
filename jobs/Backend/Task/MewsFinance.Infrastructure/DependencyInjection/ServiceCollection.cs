using MewsFinance.Application.Clients;
using MewsFinance.Infrastructure.CnbFinancialClient.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace MewsFinance.Infrastructure.DependencyInjection
{
    public static class ServiceCollection
    {
        public static void AddInfrastructure(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpClient<IFinancialClient, CnbFinancialClient.CnbFinancialClient>();
            serviceCollection.AddAutoMapper(typeof(CnbFinancialClientProfile));
        }
    }
}
