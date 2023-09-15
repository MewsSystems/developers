using Infrastructure.Services.Abstract;
using Infrastructure.Services.Concrete;
using Moq;

namespace Infrastructure.Services.Tests.Unit.Fixture;

internal class ExchangeRateProviderFixture
{
    internal Mock<IBankDataService> BankDataService;

    public ExchangeRateProviderFixture()
    {
        BankDataService = new Mock<IBankDataService>();
    }

    internal ExchangeRateProvider CreateInstance()
    {
        return new ExchangeRateProvider(BankDataService.Object);
    }
}
