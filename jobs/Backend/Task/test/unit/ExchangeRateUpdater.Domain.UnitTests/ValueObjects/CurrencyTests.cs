using System;
using ExchangeRateUpdater.Domain.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace ExchangeRateUpdater.Domain.UnitTests.ValueObjects
{
    public class CurrencyTests
    {
        [TestCase("USD")]
        [TestCase("EUR")]
        public void From_ShouldCreateCurrency_IfValueIsValidThreeLettersIsoCode(string value)
        {
            Currency.From(value).Should().NotBeNull();
        }
        
        [TestCase(null)]
        [TestCase("")]
        [TestCase("U")]
        [TestCase("US")]
        [TestCase("usd")]
        [TestCase("USDD")]
        public void From_ShouldThrowArgumentException_IfValueIsInvalidThreeLettersIsoCode(string value)
        {
            Action act = () => Currency.From(value);
            act.Should().Throw<ArgumentException>();
        }
    }
}