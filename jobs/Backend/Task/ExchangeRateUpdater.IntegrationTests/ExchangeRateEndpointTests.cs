using System.Net;

namespace ExchangeRate.IntegrationTests;

public class ExchangeRateEndpointTests : ApiTestBase
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task ExchangeRateEndpoint_GetDailyRate_ReturnsOkResponse()
    {
        var response = await Client.GetAsync("/exrates/cnb");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}