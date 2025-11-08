using ExchangeRateProviders.Core.Exception;
using NUnit.Framework;

namespace ExchangeRateProviders.Tests.Core;

[TestFixture]
public class InvalidCurrencyExceptionTests
{
    [Test]
    public void Ctor_WithInvalidCodes_SetsMessageAndCodes()
    {
        // Arrange
        var invalid = new [] { "XYZ", "ABC" };

        // Act
        var ex = new InvalidCurrencyException(invalid);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(ex.InvalidCurrencyCodes, Is.EquivalentTo(invalid));
            Assert.That(ex.Message, Is.EqualTo("Invalid currency codes: XYZ, ABC"));
            Assert.That(ex.InnerException, Is.Null);
        });
    }

    [Test]
    public void Ctor_WithInnerException_PopulatesInner()
    {
        // Arrange
        var inner = new ArgumentException("inner boom");
        var invalid = new [] { "ZZZ" };

        // Act
        var ex = new InvalidCurrencyException(invalid, inner);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(ex.InvalidCurrencyCodes, Is.EquivalentTo(invalid));
            Assert.That(ex.Message, Is.EqualTo("Invalid currency codes: ZZZ"));
            Assert.That(ex.InnerException, Is.SameAs(inner));
        });
    }

    [Test]
    public void InvalidCurrencyCodes_IsReadOnlySnapshot()
    {
        // Arrange
        var list = new List<string> { "BAD" };
        var ex = new InvalidCurrencyException(list);

        // Mutate original list after construction
        list.Add("NEW");

        // Assert original snapshot unaffected
        Assert.Multiple(() =>
        {
            Assert.That(ex.InvalidCurrencyCodes, Has.Count.EqualTo(1));
            Assert.That(ex.InvalidCurrencyCodes, Does.Not.Contain("NEW"));
        });
    }
}
