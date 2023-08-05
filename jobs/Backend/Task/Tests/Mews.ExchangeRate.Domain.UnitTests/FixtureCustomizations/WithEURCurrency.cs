using AutoFixture;

namespace Mews.ExchangeRate.Domain.UnitTests.FixtureCustomizations;
public class WithEURCurrency : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Inject(new Currency("EUR"));
    }
}
