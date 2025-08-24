using System.Net;
using System.Text;
using System.Text.Json;
using ExchangeRateProviders.Czk.Clients;
using ExchangeRateProviders.Czk.Model;
using ExchangeRateProviders.Tests.TestHelpers;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Polly;

namespace ExchangeRateProviders.Tests.Czk.Clients;

[TestFixture]
public class CzkCnbApiClientTests
{
    [Test]
    public async Task GetDailyRatesRawAsync_ValidResponse_ReturnsRates()
    {
        // Arrange
        var logger = Substitute.For<ILogger<CzkCnbApiClient>>();
        var responseData = new CnbApiCzkExchangeRateResponse
        {
            Rates = new List<CnbApiExchangeRateDto>
            {
                new() { CurrencyCode = "USD", Amount = 1, Rate = 22.50m, ValidFor = DateTime.UtcNow },
                new() { CurrencyCode = "EUR", Amount = 1, Rate = 24.00m, ValidFor = DateTime.UtcNow }
            }
        };
        var json = JsonSerializer.Serialize(responseData, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        var handler = new TestHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        });
        var httpClient = new HttpClient(handler);
        var client = new CzkCnbApiClient(httpClient, logger, ZeroBackoffRetry());

        // Act
        var result = (await client.GetDailyRatesRawAsync()).ToList();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[0].CurrencyCode, Is.EqualTo("USD"));
            Assert.That(result[0].Rate, Is.EqualTo(22.50m));
            Assert.That(result[1].CurrencyCode, Is.EqualTo("EUR"));
            Assert.That(result[1].Rate, Is.EqualTo(24.00m));
            Assert.That(handler.CallCount, Is.EqualTo(1));
            logger.VerifyLogInformation(1, "Requesting CNB rates from https://api.cnb.cz/cnbapi/exrates/daily?lang=EN");
            logger.VerifyLogInformation(1, "CNB returned 2 raw rates.");
        });
    }

    [Test]
    public async Task GetDailyRatesRawAsync_EmptyRates_ReturnsEmptyList()
    {
        // Arrange
        var logger = Substitute.For<ILogger<CzkCnbApiClient>>();
        var responseData = new CnbApiCzkExchangeRateResponse { Rates = new List<CnbApiExchangeRateDto>() };
        var json = JsonSerializer.Serialize(responseData);
        var handler = new TestHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        });
        var httpClient = new HttpClient(handler);
        var client = new CzkCnbApiClient(httpClient, logger, ZeroBackoffRetry());

        // Act
        var result = (await client.GetDailyRatesRawAsync()).ToList();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Empty);
            Assert.That(handler.CallCount, Is.EqualTo(1));
            logger.VerifyLogInformation(1, "Requesting CNB rates from https://api.cnb.cz/cnbapi/exrates/daily?lang=EN");
            logger.VerifyLogWarning(1, "CNB rates response empty.");
        });
    }

    [Test]
    public async Task GetDailyRatesRawAsync_ServerError_RetriesAndSucceeds()
    {
        // Arrange
        var logger = Substitute.For<ILogger<CzkCnbApiClient>>();
        var responseData = new CnbApiCzkExchangeRateResponse
        {
            Rates = new List<CnbApiExchangeRateDto>
            {
                new() { CurrencyCode = "JPY", Amount = 100, Rate = 17.00m, ValidFor = DateTime.UtcNow }
            }
        };
        var json = JsonSerializer.Serialize(responseData);
        var handler = new TestHttpMessageHandler(
            new HttpResponseMessage(HttpStatusCode.InternalServerError),
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });
        var httpClient = new HttpClient(handler);
        var client = new CzkCnbApiClient(httpClient, logger, ZeroBackoffRetry());

        // Act
        var result = (await client.GetDailyRatesRawAsync()).ToList();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].CurrencyCode, Is.EqualTo("JPY"));
            Assert.That(handler.CallCount, Is.EqualTo(2)); // Initial call + 1 retry
            logger.VerifyLogInformation(1, "Requesting CNB rates from https://api.cnb.cz/cnbapi/exrates/daily?lang=EN");
            logger.VerifyLogInformation(1, "CNB returned 1 raw rates.");
        });
    }

    [Test]
    public void GetDailyRatesRawAsync_TooManyRequests_RetriesAndThrows()
    {
        // Arrange
        var logger = Substitute.For<ILogger<CzkCnbApiClient>>();
        var handler = new TestHttpMessageHandler(
            new HttpResponseMessage(HttpStatusCode.TooManyRequests),
            new HttpResponseMessage(HttpStatusCode.TooManyRequests),
            new HttpResponseMessage(HttpStatusCode.TooManyRequests),
            new HttpResponseMessage(HttpStatusCode.TooManyRequests));
        var httpClient = new HttpClient(handler);
        var client = new CzkCnbApiClient(httpClient, logger, ZeroBackoffRetry());

        // Act & Assert
        Assert.Multiple(() =>
        {
            Assert.ThrowsAsync<HttpRequestException>(() => client.GetDailyRatesRawAsync());
            Assert.That(handler.CallCount, Is.EqualTo(4)); // Initial + 3 retries
            logger.VerifyLogInformation(1, "Requesting CNB rates from https://api.cnb.cz/cnbapi/exrates/daily?lang=EN");
            logger.VerifyLogInformationNotContaining("CNB returned");
            logger.VerifyLogWarningNotContaining("CNB rates response empty");
        });
    }

    [Test]
    public void GetDailyRatesRawAsync_RespectsCancellationToken()
    {
        // Arrange
        var logger = Substitute.For<ILogger<CzkCnbApiClient>>();
        var handler = new DelayHttpMessageHandler(TimeSpan.FromSeconds(5));
        var httpClient = new HttpClient(handler);
        var client = new CzkCnbApiClient(httpClient, logger, ZeroBackoffRetry());
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

        // Act & Assert
        Assert.Multiple(() =>
        {
            Assert.That(async () => await client.GetDailyRatesRawAsync(cts.Token), 
                Throws.InstanceOf<OperationCanceledException>());
            Assert.That(handler.CallCount, Is.EqualTo(1));
            
            // Verify exact log message that was logged (before cancellation)
            logger.VerifyLogInformation(1, "Requesting CNB rates from https://api.cnb.cz/cnbapi/exrates/daily?lang=EN");
            logger.VerifyLogInformationNotContaining("CNB returned");
            logger.VerifyLogWarningNotContaining("CNB rates response empty");
        });
    }

	private static IAsyncPolicy<HttpResponseMessage> ZeroBackoffRetry(int retries = 3) =>
		Policy<HttpResponseMessage>
			.Handle<HttpRequestException>()
			.OrResult(r => (int)r.StatusCode >= 500 || (int)r.StatusCode == 429)
			.WaitAndRetryAsync(retries, _ => TimeSpan.Zero);

	private class TestHttpMessageHandler : HttpMessageHandler
    {
        private readonly Queue<HttpResponseMessage> _responses;
        public int CallCount { get; private set; }

        public TestHttpMessageHandler(params HttpResponseMessage[] responses)
        {
            _responses = new Queue<HttpResponseMessage>(responses);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            CallCount++;
            var response = _responses.Count > 0 ? _responses.Dequeue() : new HttpResponseMessage(HttpStatusCode.NotFound);
            return Task.FromResult(response);
        }
    }

    private class DelayHttpMessageHandler : HttpMessageHandler
    {
        private readonly TimeSpan _delay;
        public int CallCount { get; private set; }

        public DelayHttpMessageHandler(TimeSpan delay)
        {
            _delay = delay;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            CallCount++;
            await Task.Delay(_delay, cancellationToken);
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"rates":[]}""", Encoding.UTF8, "application/json")
            };
        }
    }
}