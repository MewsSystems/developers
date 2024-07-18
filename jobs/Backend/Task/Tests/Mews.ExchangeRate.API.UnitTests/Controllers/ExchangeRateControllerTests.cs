using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Idioms;
using AutoMapper;
using Castle.Core.Logging;
using FluentAssertions;
using Mews.ExchangeRate.API.Controllers;
using Mews.ExchangeRate.API.Dtos;
using Mews.ExchangeRate.Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;

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
}
