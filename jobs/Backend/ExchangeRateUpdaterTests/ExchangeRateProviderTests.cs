using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using ExchangeRateUpdater;

using NUnit.Framework;

namespace ExchangeRateUpdaterTests
{


    [TestFixture]
    public class ExchangeRateProviderTests
    {
        private const string FAKE_URL = "http://a.com";

        [Test]
        public async Task GetExchangeRates_ReturnsCorrectRates()
        {
            var serializationCulture = new CultureInfo("en-US");
            var expectedRates = new[]
            {
                15.52m,
                20m,
                0.212m
            };
            var response = $@"12 Jul 2021 #132
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|{expectedRates[0].ToString(serializationCulture)}
Brazil |real|1|BRL|{expectedRates[1].ToString(serializationCulture)}
Croatia|kuna|1|HRK|{expectedRates[2].ToString(serializationCulture)}";

            using (var httpClient = CreateHttpClient(response))
            {
                var exchangeRateProvider = new ExchangeRateProvider(httpClient, FAKE_URL);
                var currencies = new[]
                {
                    new Currency("AUD"),
                    new Currency("BRL"),
                    new Currency("HRK"),
                };

                var rates = (await exchangeRateProvider.GetExchangeRates(currencies)).ToList();

                Assert.That(rates.Count, Is.EqualTo(currencies.Length));
                Assert.Multiple(() =>
                {
                    for (var i = 0; i < currencies.Length; i++)
                    {
                        var rate = rates[i];
                        Assert.That(rate.SourceCurrency.Code, Is.EqualTo(currencies[i].Code));
                        Assert.That(rate.TargetCurrency.Code, Is.EqualTo("CZK"));
                        Assert.That(rate.Value, Is.EqualTo(expectedRates[i]));
                    }
                });
            }
        }

        [Test]
        public async Task GetExchangeRates_AmountIsNot1_ReturnsCorrectRate()
        {
            var amount = 10;
            var rate = 20;

            var response = $@"12 Jul 2021 #132
Country|Currency|Amount|Code|Rate
Norway|krone|{amount}|NOK|{rate}";

            using (var httpClient = CreateHttpClient(response))
            {
                var exchangeRateProvider = new ExchangeRateProvider(httpClient, FAKE_URL);
                var currencies = new[]
                {
                    new Currency("NOK")
                };

                var rates = await exchangeRateProvider.GetExchangeRates(currencies);

                Assert.Multiple(() =>
                {
                    Assert.That(rates.Count(), Is.EqualTo(1));
                    Assert.That(rates.First().Value, Is.EqualTo(rate / amount));
                });
            }
        }

        [Test]
        public async Task GetExchangeRates_NoCurrenciesInTheSourceMatch_ReturnsAnEmptyCollection()
        {
            var response = @"12 Jul 2021 #132
Country|Currency|Amount|Code|Rate
Hongkong|dollar|1|HKD|2.795
Hungary|forint|100|HUF|7.240
Iceland|krona|100|ISK|17.584
Mexico|peso|1|MXN|1.090
New Zealand|dollar|1|NZD|15.118
Norway|krone|1|NOK|2.491";

            using (var httpClient = CreateHttpClient(response))
            {
                var exchangeRateProvider = new ExchangeRateProvider(httpClient, FAKE_URL);
                var currencies = new[]
                {
                    new Currency("AUD"),
                    new Currency("BRL"),
                    new Currency("CHF"),
                    new Currency("USD")
                };

                var rates = await exchangeRateProvider.GetExchangeRates(currencies);

                CollectionAssert.IsEmpty(rates);
            }
        }

        [Test]
        public void GetExchangeRates_SourceContainsAnInvalidRow_ThrowsExchangeRatesSourceException()
        {
            var response = @"12 Jul 2021 #132
Country|Currency|Amount|Code|Rate
Australia|dollar|A|AUD";

            using (var httpClient = CreateHttpClient(response))
            {
                var exchangeRateProvider = new ExchangeRateProvider(httpClient, FAKE_URL);
                Assert.ThrowsAsync<ExchangeRatesSourceException>(async () => await exchangeRateProvider.GetExchangeRates(Enumerable.Empty<Currency>()));
            }
        }

        [Test]
        public void GetExchangeRates_SourceContainsAnInvalidAmountValue_ThrowsExchangeRatesSourceException()
        {
            var response = @"12 Jul 2021 #132
Country|Currency|Amount|Code|Rate
Australia|dollar|A|AUD|10";

            using (var httpClient = CreateHttpClient(response))
            {
                var exchangeRateProvider = new ExchangeRateProvider(httpClient, FAKE_URL);
                Assert.ThrowsAsync<ExchangeRatesSourceException>(async () => await exchangeRateProvider.GetExchangeRates(Enumerable.Empty<Currency>()));
            }
        }

        [Test]
        public void GetExchangeRates_SourceContainsAnInvalidRateValue_ThrowsExchangeRatesSourceException()
        {
            var response = @"12 Jul 2021 #132
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|A";

            using (var httpClient = CreateHttpClient(response))
            {
                var exchangeRateProvider = new ExchangeRateProvider(httpClient, FAKE_URL);
                Assert.ThrowsAsync<ExchangeRatesSourceException>(async () => await exchangeRateProvider.GetExchangeRates(Enumerable.Empty<Currency>()));
            }
        }

        [Test]
        public void GetExchangeRates_SourceIsNotAvailable_ThrowsExchangeRatesSourceException()
        {
            using (var httpClient = CreateHttpClient(string.Empty, HttpStatusCode.NotFound))
            {
                var exchangeRateProvider = new ExchangeRateProvider(httpClient, FAKE_URL);
                Assert.ThrowsAsync<ExchangeRatesSourceException>(async () => await exchangeRateProvider.GetExchangeRates(Enumerable.Empty<Currency>()));
            }
        }

        private HttpClient CreateHttpClient(string response, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var httpMessageHandler = new FakeHttpMessageHandler(response, statusCode);
            return new HttpClient(httpMessageHandler);
        }

        private class FakeHttpMessageHandler : HttpMessageHandler
        {
            private readonly string response;
            private readonly HttpStatusCode statusCode;

            public FakeHttpMessageHandler(string response, HttpStatusCode statusCode)
            {
                this.response = response;
                this.statusCode = statusCode;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new HttpResponseMessage(statusCode)
                {
                    Content = new StringContent(response)
                });
            }
        }
    }
}