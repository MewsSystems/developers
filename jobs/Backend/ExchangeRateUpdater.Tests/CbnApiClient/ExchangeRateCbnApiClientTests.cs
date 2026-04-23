using System.Net;
using ExchangeRateUpdater.CbnApiClient.Implementation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace ExchangeRateUpdater.Tests.CbnApiClient
{
    public class ExchangeRateCbnApiClientTests
    {
        private IConfiguration config = null!;
        private readonly ILogger<ExchangeRateCnbApiClient> logger = NullLogger<ExchangeRateCnbApiClient>.Instance;

        [SetUp]
        public void SetUp()
        {
            config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> { { "CbnApiUrl", "https://api.cnb.cz/cnbapi/exrates/daily" } })
                .Build();
        }

        private static IHttpClientFactory CreateHttpClientFactory(HttpResponseMessage response, Action<HttpRequestMessage>? inspect = null)
        {
            // Create an in-memory HttpClient that uses a delegating handler returning the provided response.
            // Do not set a real BaseAddress to avoid any accidental network usage in tests.
            var handler = new DelegatingHandlerStub(response, inspect);
            var client = new HttpClient(handler);

            // Use NSubstitute to create a test double for IHttpClientFactory that returns our client.
            var factory = Substitute.For<IHttpClientFactory>();
            factory.CreateClient(Arg.Any<string>()).Returns(client);
            return factory;
        }

        [Test]
        public async Task GetExchangeRatesAsync_ReturnsMappedRates()
        {
            var json = "{\"rates\":[{\"amount\":100,\"country\":\"United States\",\"currency\":\"Dollar\",\"currencyCode\":\"USD\",\"order\":1,\"rate\":2500,\"validFor\":\"2026-02-15\"}]}";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json)
            };

            var factory = CreateHttpClientFactory(response);

            var client = new ExchangeRateCnbApiClient(factory, config, logger);
            var rates = await client.GetExchangeRatesAsync();

            Assert.That(rates, Is.Not.Null);
            var first = rates.First();
            Assert.That(first.SourceCurrency.Code, Is.EqualTo("USD"));
            Assert.That(first.TargetCurrency.Code, Is.EqualTo("CZK"));
            Assert.That(first.Value, Is.EqualTo(25.0m));
            Assert.That(first.ValidFor, Is.EqualTo(new DateTime(2026, 02, 15)));
        }

        [Test]
        public void GetExchangeRatesAsync_ThrowsOnNonSuccess()
        {
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var factory = CreateHttpClientFactory(response);

            var client = new ExchangeRateCnbApiClient(factory, config, logger);
            Assert.That(async () => await client.GetExchangeRatesAsync(), Throws.InstanceOf<HttpRequestException>());
        }

        [Test]
        public void GetExchangeRatesAsync_ThrowsOnEmptyContent()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(string.Empty) };
            var factory = CreateHttpClientFactory(response);

            var client = new ExchangeRateCnbApiClient(factory, config, logger);
            Assert.That(async () => await client.GetExchangeRatesAsync(), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void GetExchangeRatesAsync_ThrowsOnInvalidJson()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("not json") };
            var factory = CreateHttpClientFactory(response);

            var client = new ExchangeRateCnbApiClient(factory, config, logger);
            Assert.That(async () => await client.GetExchangeRatesAsync(), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public async Task GetExchangeRatesAsync_IncludesDateParameterWhenRequested()
        {
            HttpRequestMessage? captured = null;
            var json = "{\"rates\":[]}";
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(json) };
            var factory = CreateHttpClientFactory(response, req => captured = req);

            var client = new ExchangeRateCnbApiClient(factory, config, logger);
            var date = new DateTime(2026, 02, 15);
            await client.GetExchangeRatesAsync(date);

            Assert.That(captured, Is.Not.Null);
            Assert.That(captured!.RequestUri!.ToString(), Does.Contain("date=2026-02-15"));
        }
    }

    // Simple IHttpClientFactory implementation for tests
    internal class SimpleHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient _client;

        public SimpleHttpClientFactory(HttpClient client) => _client = client;

        public HttpClient CreateClient(string name) => _client;
    }

    // DelegatingHandler stub that returns a configured response and allows inspection of the request
    internal class DelegatingHandlerStub : DelegatingHandler
    {
        private readonly HttpResponseMessage _response;
        private readonly Action<HttpRequestMessage>? _inspect;

        public DelegatingHandlerStub(HttpResponseMessage response, Action<HttpRequestMessage>? inspect = null)
        {
            _response = response;
            _inspect = inspect;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _inspect?.Invoke(request);
            return Task.FromResult(_response);
        }
    }
}
