using ExchangeRateUpdater.Providers;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ExchangeRateUpdater.Tests
{
    public class CNBExchangeRateProviderTests
    {
        [Fact]
        public async void GetExchangeRates_ShouldReturnRates_WhenUrlIsCorrect()
        {
            //setup             
            var loggerMock = new Mock<ILogger>();

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(@"18 Mar 2022 #55
                                                Country | Currency | Amount | Code | Rate
                                                Australia | dollar | 1 | AUD | 16.621
                                                Brazil | real | 1 | BRL | 4.454
                                                Bulgaria | lev | 1 | BGN | 12.702
                                                Canada | dollar | 1 | CAD | 17.859
                                                China | renminbi | 1 | CNY | 3.548
                                                Croatia | kuna | 1 | HRK | 3.283
                                                Denmark | krone | 1 | DKK | 3.338
                                                EMU | euro | 1 | EUR | 24.840
                                                Hongkong | dollar | 1 | HKD | 2.885
                                                Hungary | forint | 100 | HUF | 6.618"),
               })
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://uri.com"),
            };

            CNBExchangeRateProvider cnbProvider = new CNBExchangeRateProvider(httpClient, loggerMock.Object);

            //act
            var rates = await cnbProvider.GetExchangeRates();

            //assert
            var audExchageReate = new ExchangeRate(new Currency("CZK"), new Currency("AUD"), (decimal)16.621);
            var hufExchangeRate = new ExchangeRate(new Currency("CZK"), new Currency("HUF"), (decimal)6.618/100);

            Assert.Equal(20, rates.Count()); //20 due to calling fetch 2times for two different sources of CNB rates
            Assert.Equal(audExchageReate.ToString() ,rates.First().ToString());
            Assert.Equal(hufExchangeRate.ToString(), rates.Last().ToString());

        }
    }
}
