using Application;
using Domain;
using Infrastructure.CnbProvider;
using Moq;
using Moq.Protected;

namespace Infrastructure.UnitTests.CnbProvider
{
    public class CnbExchangeRateProviderTest
    {
        private const string CnbResponse = @"10.06.2022 #113
země|měna|množství|kód|kurz
Indie|rupie|100|INR|30,003
USA|dolar|1|USD|23,355";

        private readonly ExchangeRate UsdExchangeRate = new ExchangeRate(new Currency("USD"), new Currency("CZK"), new decimal(23.355));
        private readonly ExchangeRate InrExchangeRate = new(new Currency("INR"), new Currency("CZK"), new decimal(0.30003));
        private readonly Currency CzkCurrency = new("CZK");

        [Fact]
        public async Task GetExchangeRates_TwoExistingCurrencies_ReturnsBothCurrencies()
        {
            IExchangeRateProvider exchangeRateProvider = new CnbExchangeRateProvider(CreateHttpClientMock());

            IEnumerable<ExchangeRate> results = await exchangeRateProvider.GetExchangeRates(new[]
            {
                UsdExchangeRate.SourceCurrency,
                InrExchangeRate.SourceCurrency
            });

            Assert.Equal(2, results.Count());
            Assert.Equal(UsdExchangeRate, results.ElementAt(0));
            Assert.Equal(InrExchangeRate, results.ElementAt(1));
        }

        [Fact]
        public async Task GetExchangeRates_OneOfTwoCurrenciesExists_ReturnsOneExistingCurrency()
        {
            IExchangeRateProvider exchangeRateProvider = new CnbExchangeRateProvider(CreateHttpClientMock());

            IEnumerable<ExchangeRate> results = await exchangeRateProvider.GetExchangeRates(new[]
            {
                UsdExchangeRate.SourceCurrency,
                CzkCurrency
            });

            Assert.Single(results);
            Assert.Equal(UsdExchangeRate, results.ElementAt(0));
        }

        [Fact]
        public async Task GetExchangeRates_OneNonExistingCurrency_ReturnsEmptyEnumerable()
        {
            IExchangeRateProvider exchangeRateProvider = new CnbExchangeRateProvider(CreateHttpClientMock());

            IEnumerable<ExchangeRate> results = await exchangeRateProvider.GetExchangeRates(new[]
            {
                CzkCurrency
            });

            Assert.Empty(results);
        }

        [Fact]
        public async Task GetExchangeRates_NoCurrencies_ReturnsEmptyEnumerable()
        {
            IExchangeRateProvider exchangeRateProvider = new CnbExchangeRateProvider(CreateHttpClientMock());

            IEnumerable<ExchangeRate> results = await exchangeRateProvider.GetExchangeRates(Enumerable.Empty<Currency>());

            Assert.Empty(results);
        }

        private HttpClient CreateHttpClientMock()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                {
                    HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    response.Content = new StringContent(CnbResponse);
                    return response;
                });

            return new HttpClient(mockHttpMessageHandler.Object);
        }
    }
}