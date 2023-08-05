using AutoFixture;
using AutoFixture.Idioms;
using FluentAssertions;
using Mews.ExchangeRate.Domain.UnitTests.FixtureCustomizations;

namespace Mews.ExchangeRate.Domain.UnitTests;
public class ExchangeRateUnitTests
{
    private Fixture _fixture;

    public ExchangeRateUnitTests()
    {
        _fixture = new Fixture();
        _fixture.CustomizeWithCurrencyEur();
    }

    [Fact]
    public void Constructor_IsGuardedAgainst_Null()
    {
        var assertion = _fixture.Create<GuardClauseAssertion>();
        assertion.Verify(typeof(ExchangeRate).GetConstructors());
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Constructor_IsGuardedAgainst_NegativeOrZeroValue(decimal value)
    {
         var sut = ()=> new ExchangeRate(_fixture.Create<Currency>(), 
             _fixture.Create<Currency>(),
             value);

        sut.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Equals_ReturnsTrue_WhenComparingWithItself()
    {
        var assertion = _fixture.Create<EqualsSelfAssertion>();
        assertion.Verify(typeof(ExchangeRate));
    }

    [Fact]
    public void Equals_ReturnsFalse_WhenDifferent()
    {
        var assertion = _fixture.Create<EqualsNewObjectAssertion>();
        assertion.Verify(typeof(ExchangeRate));
    }

    [Fact]
    public void Equals_ReturnsFalse_WhenNull()
    {
        var assertion = _fixture.Create<EqualsNullAssertion>();
        assertion.Verify(typeof(ExchangeRate));
    }

    [Fact]
    public void Equals_ReturnsSameValue_WhenCallingMultipleTimes()
    {
        var assertion = _fixture.Create<EqualsSuccessiveAssertion>();
        assertion.Verify(typeof(ExchangeRate));
    }

    [Fact]
    public void GetHashCode_ReturnsSameValue_WhenCallingMultipleTimes()
    {
        var assertion = _fixture.Create<GetHashCodeSuccessiveAssertion>();
        assertion.Verify(typeof(ExchangeRate));
    }

    [Fact]
    public void ToString_ShouldReturnTheSourceExchangeDividedByTheTargetCurrency()
    {
        var sut = _fixture.Create<ExchangeRate>();

        sut.ToString()
        .Should()
            .Be($"{sut.SourceCurrency}/{sut.TargetCurrency}={sut.Value}");
    }
}
