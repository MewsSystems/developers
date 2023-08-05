using AutoFixture;

namespace Mews.ExchangeRate.Domain.UnitTests.FixtureCustomizations;
internal static class CustomizationExtensionMethods
{
    public static IFixture CustomizeWithCurrencyEur(this IFixture fixture)
    {
        return fixture.Customize(new WithCurrencyEur());
    }
}
