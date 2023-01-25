using ExchangeRateUpdater.AzFunction.Logic.ExchangeRateProvider;
using ExchangeRateUpdater.AzFunction.Setup;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace ExchangeRateUpdater.AzFunction.Setup
{

    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // The thing here is that we need to inject the dependencies for our logic in order to be able to test test it.
            // Keeping it simple. Additionally, this class will receive a IHttpClientFactory parameter that will initiate the HttpClient
            // so in a production environments make sense to make this a singleton. Framework will manage the HttpClients automatically.
            builder.Services.AddSingleton<IExchangeRateProviderManager, ExchangeRateProviderManager>();
            

        }
    }
}
