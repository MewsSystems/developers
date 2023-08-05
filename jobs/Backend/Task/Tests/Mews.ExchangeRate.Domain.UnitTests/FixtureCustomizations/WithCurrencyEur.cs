using AutoFixture;

namespace Mews.ExchangeRate.Domain.UnitTests.FixtureCustomizations;
public class WithCurrencyEur : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Inject(new Currency("EUR"));
    }
}
