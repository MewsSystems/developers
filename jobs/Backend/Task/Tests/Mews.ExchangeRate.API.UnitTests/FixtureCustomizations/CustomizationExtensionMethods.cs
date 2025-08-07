using AutoFixture;

namespace Mews.ExchangeRate.API.UnitTests.FixtureCustomizations;
internal static class CustomizationExtensionMethods
{
    public static IFixture CustomizeWithEURCurrencyDto(this IFixture fixture)
    {
        return fixture.Customize(new WithEURCurrencyDto());
    }
}
