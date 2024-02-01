using ExchangeRateUpdater.Domain.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace ExchangeRateUpdater.Domain.Tests.Unit.ValueObjectTests
{
    [TestFixture]
    internal class CurrencyTests
    {
        [TestCase("ABC")]
        [TestCase("USD_")]
        [TestCase("A")]
        public void GivenNoISO4217CurrencyCode_ShouldThrowFormatException(string currencyCode)
        {
            // act & assert
            var formatException = Assert.Throws<FormatException>(() => CreateSut(currencyCode));

            formatException!.Message.Should().Be("currencyCode has to be of ISO 4217 format and considered active according to https://en.wikipedia.org/wiki/ISO_4217");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void GivenAnInvalidCurrencyCode_ShouldThrowArgumentNullException(string? currencyCode)
        {
            // act & assert
            var formatException = Assert.Throws<ArgumentNullException>(() => CreateSut(currencyCode));

            formatException!.ParamName.Should().Be("currencyCode cannot be empty, null or whitespaces");
        }

        [TestCase("CZK")]
        [TestCase("USD")]
        [TestCase("EUR")]
        public void GivenAISO4217CurrencyCode_ShouldSucceedInCreatingCurrency(string currencyCode)
        {
            // act & assert
            var sut = CreateSut(currencyCode);

            sut.CurrencyCode.Should().Be(currencyCode);
        }

        private Currency CreateSut(string? currencyCode)
        {
            return new Currency(currencyCode);
        }
    }
}
