using AutoFixture;

namespace Mews.ExchangeRate.Domain.UnitTests.FixtureCustomizations;
internal static class CustomizationExtensionMethods
{
    public static IFixture CustomizeWithEURCurrency(this IFixture fixture)
    {
        return fixture.Customize(new WithEURCurrency());
    }

    public static IFixture CustomizeWithEURCurrencyAndExchangeRate(this IFixture fixture)
    {
        return fixture
            .Customize(new WithEURCurrency())
            .Customize(new WithEURExchangeRate());
    }
}
