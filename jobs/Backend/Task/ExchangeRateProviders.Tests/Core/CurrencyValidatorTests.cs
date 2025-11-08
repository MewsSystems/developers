using ExchangeRateProviders.Core;
using ExchangeRateProviders.Core.Exception;
using ExchangeRateProviders.Core.Model;
using NUnit.Framework;

namespace ExchangeRateProviders.Tests.Core;

[TestFixture]
public class CurrencyValidatorTests
{
    [Test]
    public void ValidateCurrencyCodes_ValidCurrencies_DoesNotThrow()
    {
        // Arrange
        var currencies = new[]
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("GBP"),
            new Currency("JPY")
        };

        // Act & Assert
        Assert.DoesNotThrow(() => CurrencyValidator.ValidateCurrencyCodes(currencies));
    }

    [Test]
    public void ValidateCurrencyCodes_NullCollection_DoesNotThrow()
    {
        // Act & Assert
        Assert.DoesNotThrow(() => CurrencyValidator.ValidateCurrencyCodes(null!));
    }

    [Test]
    public void ValidateCurrencyCodes_EmptyCollection_DoesNotThrow()
    {
        // Arrange
        var currencies = Array.Empty<Currency>();

        // Act & Assert
        Assert.DoesNotThrow(() => CurrencyValidator.ValidateCurrencyCodes(currencies));
    }

    [Test]
    public void ValidateCurrencyCodes_SingleInvalidCurrency_ThrowsInvalidCurrencyException()
    {
        // Arrange
        var currencies = new[]
        {
            new Currency("USD"),
            new Currency("XYZ") // Invalid currency
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidCurrencyException>(() => 
            CurrencyValidator.ValidateCurrencyCodes(currencies));

        Assert.Multiple(() =>
        {
            Assert.That(exception!.InvalidCurrencyCodes, Has.Count.EqualTo(1));
            Assert.That(exception.InvalidCurrencyCodes, Does.Contain("XYZ"));
            Assert.That(exception.Message, Does.Contain("Invalid currency codes: XYZ"));
        });
    }

    [Test]
    public void ValidateCurrencyCodes_MultipleInvalidCurrencies_ThrowsInvalidCurrencyException()
    {
        // Arrange
        var currencies = new[]
        {
            new Currency("USD"),   // Valid
            new Currency("XYZ"),   // Invalid
            new Currency("ABC"),   // Invalid
            new Currency("EUR")    // Valid
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidCurrencyException>(() => 
            CurrencyValidator.ValidateCurrencyCodes(currencies));

        Assert.Multiple(() =>
        {
            Assert.That(exception!.InvalidCurrencyCodes, Has.Count.EqualTo(2));
            Assert.That(exception.InvalidCurrencyCodes, Does.Contain("XYZ"));
            Assert.That(exception.InvalidCurrencyCodes, Does.Contain("ABC"));
            Assert.That(exception.Message, Does.Contain("Invalid currency codes: XYZ, ABC"));
        });
    }

    [Test]
    public void ValidateCurrencyCodes_NullCurrencyCode_ThrowsInvalidCurrencyException()
    {
        // Arrange
        var currencies = new[]
        {
            new Currency("USD"),
            new Currency(null!) // This will create a currency with null code
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidCurrencyException>(() => 
            CurrencyValidator.ValidateCurrencyCodes(currencies));

        Assert.Multiple(() =>
        {
            Assert.That(exception!.InvalidCurrencyCodes, Has.Count.EqualTo(1));
            Assert.That(exception.InvalidCurrencyCodes, Does.Contain("null"));
        });
    }

    [Test]
    public void ValidateCurrencyCodes_EmptyStringCurrencyCode_ThrowsInvalidCurrencyException()
    {
        // Arrange
        var currencies = new[]
        {
            new Currency("USD"),
            new Currency("") // Empty string
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidCurrencyException>(() => 
            CurrencyValidator.ValidateCurrencyCodes(currencies));

        Assert.That(exception!.InvalidCurrencyCodes, Has.Count.EqualTo(1));
    }

    [Test]
    public void ValidateCurrencyCodes_WhitespaceOnlyCurrencyCode_ThrowsInvalidCurrencyException()
    {
        // Arrange
        var currencies = new[]
        {
            new Currency("USD"),
            new Currency("   ") // Whitespace only
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidCurrencyException>(() => 
            CurrencyValidator.ValidateCurrencyCodes(currencies));

        Assert.That(exception!.InvalidCurrencyCodes, Has.Count.EqualTo(1));
    }
}