using NFluent;
using ExchangeRateUpdater.Contract;

namespace ExchangeRateUpdater.Test.Contract;

public sealed class CurrencyTests
{
    [Test]
    public void Constructor_ShouldSetCode_WhenValidCodeIsPassed()
    {
        // Arrange
        const string validCurrencyCode = "USD";

        // Act
        var currency = new Currency(validCurrencyCode);

        // Assert
        Check.That(currency).IsEqualTo(new Currency(validCurrencyCode));
    }

    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenNullCodeIsPassed()
    {
        // Act & Assert
        Check.ThatCode(() => new Currency(null)).Throws<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ShouldThrowArgumentException_WhenInvalidCodeIsPassed()
    {
        // Act & Assert
        Check.ThatCode(() => new Currency("US")).Throws<ArgumentException>();
    }

    [Test]
    public void ToString_ShouldReturnCode()
    {
        // Arrange
        var currency = new Currency("GBP");

        // Act
        var result = currency.ToString();

        // Assert
        Check.That(result).IsEqualTo("GBP");
    }

    [Test]
    public void ImplicitOperator_ShouldConvertFromCurrencyToString()
    {
        // Arrange
        var currency = new Currency("EUR");

        // Act
        string result = currency;

        // Assert
        Check.That(result).IsEqualTo("EUR");
    }

    [Test]
    public void ImplicitOperator_ShouldConvertFromStringToCurrency()
    {
        // Arrange
        const string code = "JPY";

        // Act
        Currency currency = code;

        // Assert
        Check.That(currency).IsEqualTo(new Currency("JPY"));
    }

    [Test]
    public void Czk_ShouldHaveCorrectCode()
    {
        // Act
        var currency = Currency.Czk;

        // Assert
        Check.That(currency).IsEqualTo(new Currency("CZK"));
    }

    [Test]
    public void CurrencyCodeRegex_ShouldMatchValidCurrencyCode()
    {
        // Act
        var isMatch = Currency.CurrencyCodeRegex.IsMatch("EUR");

        // Assert
        Check.That(isMatch).IsTrue();
    }

    [Test]
    public void CurrencyCodeRegex_ShouldNotMatchInvalidCurrencyCode()
    {
        // Act
        var isMatch = Currency.CurrencyCodeRegex.IsMatch("Eur");

        // Assert
        Check.That(isMatch).IsFalse();
    }
}