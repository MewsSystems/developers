using ExchangeRateUpdater.Common;
using Moq;
using Moq.Protected;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Cnb.Test;

[TestFixture]
public class CnbRateProviderTests
{
    public const string ValidCase = @"30 Jan 2024 #21
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|15.089
Brazil|real|1|BRL|4.625
Hungary|forint|100|HUF|6.414";

    [Test]
    public static void WhenCorrectDataReceived_CorrectFilteringApplied()
    {
        var client = CreateMockClientFor200(ValidCase);
        var sut = new CnbRateProvider(client);

        var result = 
            sut.GetExchangeRates(new[] { new Currency("AUD"), new Currency("HUF"), new Currency("XXX")})
            .ToList();

        Assert.Multiple(() => 
        {
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result, Has.One.Matches<ExchangeRate>(r => r.SourceCurrency.Code == "AUD"));
            Assert.That(result, Has.One.Matches<ExchangeRate>(r => r.SourceCurrency.Code == "HUF"));
            Assert.That(result, Has.All.Matches<ExchangeRate>(r => r.TargetCurrency.Code == "CZK"), "Target check");
            Assert.That(result, Has.None.Matches<ExchangeRate>(r => r.SourceCurrency.Code == "XXX"), "Not found not present");
        });
    }

    [Test]
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.NotFound)]
    public static void WhenNon200Response_Throws(HttpStatusCode statusCode)
    {
        var client = CreateMockClient(new HttpResponseMessage()
        {
            StatusCode = statusCode
        });

        var sut = new CnbRateProvider(client);
        Assert.That(() => sut.GetExchangeRates(new [] { new Currency("AUD") }).ToList(), Throws.Exception);
    }

    [Test]
    [TestCase("")]
    [TestCase("one row only")]
    public static void WhenResponseHasInvalidHeaderRows_Throws(string data)
    {
        var client = CreateMockClientFor200(string.Empty);

        var sut = new CnbRateProvider(client);
        Assert.That(() => sut.GetExchangeRates(new [] { new Currency("AUD") }).ToList(), Throws.TypeOf<FormatException>());
    }

#region Utility methods

    // going with HttpClient mocking
    // a "real" solution would heavily depend on the codebase (eg. if IHttpClientFactory or Polly is used)

    private static HttpClient CreateMockClientFor200(string response)
    {
        return CreateMockClient(new HttpResponseMessage()
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(response)
        });
    }

    private static HttpClient CreateMockClient(HttpResponseMessage response)
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(response);

        return new HttpClient(handlerMock.Object)
        {
            // 240.0.0.0/4 block is reserved, esentially making it a networking /dev/null
            // preventive measure even though the calls should never get through
            BaseAddress = new Uri("https://240.0.0.1/")
        };
    }

#endregion
}