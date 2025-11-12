using System.Net;
using System.Net.Http.Json;
using ExchangeRate.Domain.Providers.CzechNationalBank;
using ExchangeRate.IntegrationTests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRate.IntegrationTests;

public class ExchangeRateEndpointTests : ApiTestBase
{
    private const string BaseUrl = "exrates/cnb";

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    [TestCase("2024-01-01", null)]
    [TestCase(null, "CZ")]
    [TestCase("2024-01-01", "CZ")]
    public async Task ExchangeRateEndpoint_GetExchangeRate_ReturnsOkResponse(
        string date, string lang)
    {
        var query = QueryBuilder.BuildUriQuery(BaseUrl,
            ("date", date),
            ("lang", lang));

        var response = await Client.GetAsync(query);
        var content = await response.Content.ReadFromJsonAsync<CzechNationalBankProviderResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().NotBeNull();
    }

    [Test]
    [TestCase("0000-00-00", null)]
    [TestCase(null, "XX")]
    [TestCase("0000-00-00", "XX")]
    public async Task ExchangeRateEndpoint_GetExchangeRateWithInvalidParameters_ReturnsBadRequestResponse(
        string date, string lang)
    {
        var query = QueryBuilder.BuildUriQuery(BaseUrl,
            ("date", date),
            ("lang", lang));

        var response = await Client.GetAsync(query);
        var validationDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        validationDetails.Should().NotBeNull();
        validationDetails.Title.Should().NotBeNullOrEmpty();
        validationDetails.Errors.Should().HaveCountGreaterThan(0);
        validationDetails.Instance.Should().Be($"GET /{BaseUrl}/");
        validationDetails.Extensions.Should().ContainKey("requestId").WhoseValue.Should().NotBeNull();
        validationDetails.Extensions.Should().ContainKey("traceId").WhoseValue.Should().NotBeNull();
    }
}