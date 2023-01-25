using ExchangeRateUpdater.AzFunction.Logic.ExchangeRateProvider;
using ExchangeRateUpdater.Models;
using Moq;
using Moq.Protected;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderTests
    {
        Mock<IHttpClientFactory> _mockFactory = new Mock<IHttpClientFactory>();

        public ExchangeRateProviderTests()
        {
            _mockFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(File.ReadAllText("CNBCRateExchangeExample.xml")),
                });



            var client = new HttpClient(mockHttpMessageHandler.Object);
            _mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            AddEnvironmentSettings();
        }

        private static void AddEnvironmentSettings()
        {
            using var file = File.OpenText("local.settings.json");

            var reader = new JsonTextReader(file);
            var jObject = JObject.Load(reader);
            foreach (var variable in jObject)
            {
                Environment.SetEnvironmentVariable(variable.Key, variable.Value?.ToString());
            }
        }

        [Fact]
        public async Task Empty_Currencies_Should_Return_Empty_Result()
        {
            IExchangeRateProvider rateProvider = new ExchangeRateProvider(_mockFactory.Object, "CZK");
            var result = await rateProvider.GetExchangeRates(Enumerable.Empty<Currency>());
            Assert.True(result.Count() == 0);
        }

        [Fact]
        public async Task Non_Valid_Currencies_Should_Not_Appear_On_Result()
        {
            IEnumerable<Currency> currencies = new[] { new Currency("XYZ") };

            IExchangeRateProvider rateProvider = new ExchangeRateProvider(_mockFactory.Object, "CZK");
            var result = await rateProvider.GetExchangeRates(currencies);

            Assert.True(result.Count() == 0);
            Assert.DoesNotContain(new ExchangeRate(new Currency("XYZ"), new Currency("CZK"), (decimal)23.875), result, new CustomExchangeRateEqualityComparer());

        }

        [Fact]
        public async Task Only_Valid_Currencies_Should_Appear_On_Result()
        {
            IEnumerable<Currency> currencies = new[] { new Currency("EUR"), new Currency("USD") };

            IExchangeRateProvider rateProvider = new ExchangeRateProvider(_mockFactory.Object, "CZK");
            var result = await rateProvider.GetExchangeRates(currencies);

            Assert.True(result.Count() == 2);
            Assert.Contains(new ExchangeRate(new Currency("EUR"), new Currency("CZK"), (decimal)23.875), result, new CustomExchangeRateEqualityComparer());
            Assert.Contains(new ExchangeRate(new Currency("USD"), new Currency("CZK"), (decimal)21.987), result, new CustomExchangeRateEqualityComparer());

        }
    }

    public class CustomExchangeRateEqualityComparer : IEqualityComparer<ExchangeRate>
    {
        public bool Equals(ExchangeRate? x, ExchangeRate? y)
        {
            return string.Compare(x?.SourceCurrency.Code, y?.SourceCurrency.Code, true, CultureInfo.InvariantCulture) == 0
                && string.Compare(x?.TargetCurrency.Code, y?.TargetCurrency.Code, true, CultureInfo.InvariantCulture) == 0
                && x?.Value == y?.Value;
        }

        public int GetHashCode([DisallowNull] ExchangeRate obj)
        {
            throw new NotImplementedException();
        }
    }
}