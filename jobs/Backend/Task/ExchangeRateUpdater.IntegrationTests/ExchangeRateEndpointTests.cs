using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRate.IntegrationTests;

public class ExchangeRateEndpointTests : ApiTestBase
{
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
        var response = await Client.GetAsync($"/exrates/cnb?date={date}&lang={lang}");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    [TestCase("0000-00-00", null)]
    [TestCase(null, "XX")]
    [TestCase("0000-00-00", "XX")]
    public async Task ExchangeRateEndpoint_GetExchangeRateWithInvalidParameters_ReturnsBadRequestResponse(
        string date, string lang)
    {
        var response = await Client.GetAsync($"/exrates/cnb?date={date}&lang={lang}");
        var validationDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        Assert.NotNull(validationDetails);
    }
}