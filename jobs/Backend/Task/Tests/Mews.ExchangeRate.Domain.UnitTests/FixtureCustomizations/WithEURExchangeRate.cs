using AutoFixture;

namespace Mews.ExchangeRate.Domain.UnitTests.FixtureCustomizations;
internal class WithEURExchangeRate : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Inject(new ExchangeRate(new Currency("EUR"),
            new Currency("USD"),
            1));
    }
}