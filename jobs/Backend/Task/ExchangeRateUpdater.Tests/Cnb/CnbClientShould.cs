using System.Net;
using System.Net.Http.Json;
using ExchangeRateUpdater.Cnb;
using Microsoft.Extensions.Logging.Abstractions;

namespace ExchangeRateUpdater.Tests.Cnb;

[Trait("Category", "Unit")]
public class CnbClientShould
{
    [Fact]
    public async Task ValidateResponsePayload()
    {
        // arrange
        using var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = JsonContent.Create(
            new
            {
                rates = new[]
                {
                    new
                    {
                        validFor = new DateTime(1955, 11, 5), amount = 1,
                    }
                }
            });

        var httpClient = new HttpClient(new TestHttpMessageHandler(response));
        var client = new CnbClient(httpClient, NullLogger<CnbClient>.Instance);

        // act
        var result = await client.GetCurrentExchangeRates(CancellationToken.None);

        // assert
        Assert.True(result.TryPick(out CnbError? _));
    }

    private sealed class TestHttpMessageHandler(HttpResponseMessage response) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
            Task.FromResult(response);
    }
}