using Adapter.InMemory.Repositories;
using Domain.Ports;
using ExchangeRateUpdaterApi.Configuration;
using SimpleInjector;

namespace ExchangeRateUpdaterApi.Tests.Unit;

public class TestApplicationHostBuilder : ApplicationHostBuilder
{
    public TestApplicationHostBuilder(string[] args, string applicationName, ISettings settings) : base(args, applicationName, settings)
    {
    }
    
    public void ReplaceRegistrations(IExchangeRatesRepository exchangeRatesRepository)
    {
        Container.Options.AllowOverridingRegistrations = true;
        
        Container.RegisterInstance<IExchangeRatesRepository>(exchangeRatesRepository);
        
        Container.Options.AllowOverridingRegistrations = false;
    }
}