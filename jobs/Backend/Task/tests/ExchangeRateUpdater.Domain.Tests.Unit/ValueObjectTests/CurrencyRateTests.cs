using ExchangeRateUpdater.Domain.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace ExchangeRateUpdater.Domain.Tests.Unit.ValueObjectTests;

[TestFixture]
internal class CurrencyRateTests
{
    [TestCase(-100)]
    [TestCase(-25.5)]
    [TestCase(-0.000001)]
    [TestCase(0)]
    public void GivenAnInvalidCurrencyRate_ShouldThrowArgumentOutOfRangeException(decimal currencyRate)
    {
        // act & assert
        var argumentOutOfRangeException = Assert.Throws<ArgumentOutOfRangeException>(() => CreateSut(currencyRate));

        argumentOutOfRangeException!.ParamName.Should().Be("value has to be greater than 0.");
    }

    [TestCase(100)]
    [TestCase(25.5)]
    [TestCase(0.000001)]
    public void GivenAnValidCurrencyRate_ShouldSucceedInCreatingCurrencyRateValueObject(decimal currencyRate)
    {
        // act & assert
        var sut = CreateSut(currencyRate);

        sut.Value.Should().Be(currencyRate);
    }

    private PositiveRealNumber CreateSut(decimal currencyRate)
    {
        return new PositiveRealNumber(currencyRate);
    }
}
