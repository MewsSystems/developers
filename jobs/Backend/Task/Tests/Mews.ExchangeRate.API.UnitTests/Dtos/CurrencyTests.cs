using AutoFixture;
using FluentAssertions;
using Mews.ExchangeRate.API.UnitTests.FixtureCustomizations;
using System.ComponentModel.DataAnnotations;

namespace Mews.ExchangeRate.API.UnitTests.Dtos;
public class CurrencyTests
{
    private readonly IFixture _fixture;

    public CurrencyTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void Validate_Suceed_WhenObjectIsValid()
    {
        _fixture.CustomizeWithEURCurrencyDto();
        var sut = _fixture.Create<API.Dtos.Currency>();

        var validationResults = new List<ValidationResult>();
        var result = Validator.TryValidateObject(sut,
            new ValidationContext(sut, null, null),
            validationResults);

        result.Should()
            .BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_Fails_WhenCodeIsNullOrWhiteSpace(string currencyCode)
    {
        var sut = new API.Dtos.Currency(currencyCode);

        var validationResults = new List<ValidationResult>();
        var result = Validator.TryValidateObject(sut,
            new ValidationContext(sut, null, null),
            validationResults);

        result.Should()
            .BeFalse();
    }

    [Fact]
    public void Validate_Fails_WhenCodeIsNotISO4217()
    {
        var sut = _fixture.Create<API.Dtos.Currency>();

        var validationResults = new List<ValidationResult>();
        var result = Validator.TryValidateObject(sut,
            new ValidationContext(sut, null, null),
            validationResults);

        result.Should()
            .BeFalse();
    }
}
