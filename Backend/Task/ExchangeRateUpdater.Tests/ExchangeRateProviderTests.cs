using Moq;
using Moq.Protected;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Linq;
using System.Net.Http;

namespace ExchangeRateUpdater.Tests
{
    [TestFixture]
    public class ExchangeRateProviderTests
    {
        private ExchangeRateProvider _exchangeRateProvider;
        private readonly Currency _czkCurrency = new Currency("CZK");

        [SetUp]
        public void ExchangeRateProviderTestsSetUp()
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent("07.Mar 2019 #47\nCountry|Currency|Amount|Code|Rate\nBrazil|real|1|BRL|5.922"),
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);
            var url = "https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt";

            _exchangeRateProvider = new ExchangeRateProvider(new CnbExchangeRateResponseParser(),
                new CnbExchangeRatesFilter(), httpClient, url, _czkCurrency);
        }

        [Test]
        public void GetExchangeRatesAsyncTest1ElementEnumerable()
        {
            var currencies = new[]
            { 
                new Currency("BRL"),
                new Currency("CZK"),
            };

            var expectedResult = new[]
            {
                new ExchangeRate(new Currency("BRL"), _czkCurrency, 5.922m),
            };

            var actualResult = _exchangeRateProvider.GetExchangeRatesAsync(currencies);
            actualResult.Wait();

            CollectionAssert.AreEqual(expectedResult, actualResult.Result.ToArray(), new ExchangeRateComparer());
        }
    }
}
