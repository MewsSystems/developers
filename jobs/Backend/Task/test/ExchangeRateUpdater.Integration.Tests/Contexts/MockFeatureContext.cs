using Microsoft.Extensions.DependencyInjection;
using Moq;
using TechTalk.SpecFlow;

namespace ExchangeRateUpdater.Integration.Tests.Contexts;

public class MockFeatureContext(FeatureContext featureContext)
{
    private readonly IServiceProvider _serviceProvider = featureContext.Get<IServiceProvider>();

    public Mock<T> GetMock<T>() where T : class
    {
        return _serviceProvider.GetRequiredService<Mock<T>>();
    }

    public void ResetMocks()
    {
        var mocks = _serviceProvider.GetServices<Mock>();
        foreach (var mock in mocks)
        {
            mock.Reset();
        }
    }
}

