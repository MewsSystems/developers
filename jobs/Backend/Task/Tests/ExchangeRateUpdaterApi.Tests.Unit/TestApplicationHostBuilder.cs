using Adapter.InMemory.Repositories;
using Domain.Ports;
using SimpleInjector;

namespace ExchangeRateUpdaterApi.Tests.Unit;

public class TestApplicationHostBuilder : ApplicationHostBuilder
{
    public TestApplicationHostBuilder(string[] args, string applicationName) : base(args, applicationName)
    {
    }

    public override void RegisterDependencies(IExchangeRatesRepository exchangeRatesRepository)
    {
        Container.RegisterInstance<IExchangeRatesRepository>(exchangeRatesRepository);
    }
}