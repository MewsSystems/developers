using AutoFixture;
using Mews.ExchangeRate.API.Dtos;

namespace Mews.ExchangeRate.API.UnitTests.FixtureCustomizations;
public class WithEURCurrencyDto : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Inject(new Currency("EUR"));
    }
}
