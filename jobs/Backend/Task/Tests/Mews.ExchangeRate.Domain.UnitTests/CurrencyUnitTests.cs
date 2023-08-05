using AutoFixture;
using AutoFixture.Idioms;
using FluentAssertions;
using Mews.ExchangeRate.Domain.UnitTests.FixtureCustomizations;

namespace Mews.ExchangeRate.Domain.UnitTests;
public class CurrencyUnitTests
{
    private Fixture _fixture;

    public CurrencyUnitTests()
    {
        _fixture = new Fixture();
        _fixture.CustomizeWithEURCurrency();
    }

    [Fact]
    public void Constructor_isGuardedAgainst_nulls()
    {
        var assertion = _fixture.Create<GuardClauseAssertion>();
        assertion.Verify(typeof(Currency).GetConstructors());
    }

    [Fact]
    public void Constructor_isGuardedAgainst_EmptyStrings()
    {
        var assertion = new GuardClauseAssertion(_fixture, new EmptyStringBehaviorExpectation());
        assertion.Verify(typeof(Currency).GetConstructors());
    }

    [Fact]
    public void Constructor_isGuardedAgainst_WhiteSpaces()
    {
        var assertion = new GuardClauseAssertion(_fixture, new WhiteSpaceStringBehaviorExpectation());
        assertion.Verify(typeof(Currency).GetConstructors());
    }

    [Theory]
    [InlineData("XX")]
    [InlineData("XXXX")]
    [InlineData("XX2")]
    public void Constructor_ThrowsArgumentException_WhenCodeDoesNotFollowISO4217Format(string code)
    {
        var sut = () => new Currency(code);

        sut.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_AssignsTheCode_WhenItFollowsISO4217Format()
    {
        var sut = _fixture.Create<Currency>();

        sut.Code
            .Should()
            .Be("EUR");
    }

    [Fact]
    public void Equals_returnsTrue_whenComparingWithItself()
    {
        var assertion = _fixture.Create<EqualsSelfAssertion>();
        assertion.Verify(typeof(Currency));
    }

    [Fact]
    public void Equals_returnsFalse_whenDifferent()
    {
        var assertion = _fixture.Create<EqualsNewObjectAssertion>();
        assertion.Verify(typeof(Currency));
    }

    [Fact]
    public void Equals_returnsFalse_whenNull()
    {
        var assertion = _fixture.Create<EqualsNullAssertion>();
        assertion.Verify(typeof(Currency));
    }

    [Fact]
    public void Equals_returnsSameValue_whenCallingMultipleTimes()
    {
        var assertion = _fixture.Create<EqualsSuccessiveAssertion>();
        assertion.Verify(typeof(Currency));
    }

    [Fact]
    public void GetHashCode_returnsSameValue_whenCallingMultipleTimes()
    {
        var assertion = _fixture.Create<GetHashCodeSuccessiveAssertion>();
        assertion.Verify(typeof(Currency));
    }

    [Fact]
    public void ToString_ShouldReturnTheCode()
    {
        var sut = _fixture.Create<Currency>();

        sut.ToString()
            .Should()
            .Be(sut.Code);
    }
}
