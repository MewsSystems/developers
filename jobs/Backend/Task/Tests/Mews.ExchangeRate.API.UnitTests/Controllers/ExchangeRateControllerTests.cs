using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Idioms;
using FluentAssertions;
using Mews.ExchangeRate.API.Controllers;
using Mews.ExchangeRate.API.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Mews.ExchangeRate.API.UnitTests.Controllers;
public class ExchangeRateControllerTests
{
    private Fixture _fixture;

    public ExchangeRateControllerTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
    }

    [Fact]
    public void Constructor_isGuardedAgainst_nulls()
    {
        var assertion = _fixture.Create<GuardClauseAssertion>();
        assertion.Verify(typeof(ExchangeRateController).GetConstructors());
    }

    [Fact]
    public async Task Post_ReturnsHttp400_WhenModelIsNotValid()
    {
        var sut = _fixture.Create<ExchangeRateController>();
        var currencies = _fixture.CreateMany<Currency>();

        var result = await sut.Post(currencies);
        result.Should().BeOfType<BadRequest>();
    }
}
