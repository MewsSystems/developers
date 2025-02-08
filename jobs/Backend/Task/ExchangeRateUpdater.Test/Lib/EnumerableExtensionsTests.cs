using ExchangeRateUpdater.Lib.Collection;
using NFluent;

namespace ExchangeRateUpdater.Test.Lib;

public class EnumerableExtensionsTests
{
    [Test]
    public void IsNullOrEmpty_ShouldReturnTrue_WhenEnumerableIsNull()
    {
        // Arrange
        IEnumerable<int>? enumerable = null;

        // Act
        var result = enumerable.IsNullOrEmpty();

        // Assert
        Check.That(result).IsTrue();
    }

    [Test]
    public void IsNullOrEmpty_ShouldReturnTrue_WhenEnumerableIsEmpty()
    {
        // Arrange
        var enumerable = Enumerable.Empty<int>();

        // Act
        var result = enumerable.IsNullOrEmpty();

        // Assert
        Check.That(result).IsTrue();
    }

    [Test]
    public void IsNullOrEmpty_ShouldReturnFalse_WhenEnumerableHasElements()
    {
        // Arrange
        var enumerable = new List<int> { 1, 2, 3 };

        // Act
        var result = enumerable.IsNullOrEmpty();

        // Assert
        Check.That(result).IsFalse();
    }

    [Test]
    public void IsNullOrEmpty_ShouldReturnFalse_WhenEnumerableHasSingleElement()
    {
        // Arrange
        var enumerable = new List<int> { 42 };

        // Act
        var result = enumerable.IsNullOrEmpty();

        // Assert
        Check.That(result).IsFalse();
    }
}