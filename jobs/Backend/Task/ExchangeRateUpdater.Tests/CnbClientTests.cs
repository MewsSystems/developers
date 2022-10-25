using ExchangeRateUpdater.Cnb;
using ExchangeRateUpdater.Cnb.Dtos;
using FluentAssertions;
using Flurl.Http;
using Flurl.Http.Testing;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.Tests
{
    [TestFixture]
    public class CnbClientTests
    {
        private const string SuccessResponse = @"21 Oct 2022 #201
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|15.751
Iceland|krona|100|ISK|17.434
New Zealand|dollar|1|NZD|14.119";

        private CnbClient _cnbClient;

        [SetUp]
        public void Setup()
        {
            var logger = new Mock<ILogger<CnbClient>>();
            _cnbClient = new CnbClient(logger.Object, new CnbClient.Options { Url = "http://test.test" });
        }

        [Test]
        public async Task WhenGetLatestExchangeRatesIsSuccessThenParsesResponseCorrectly()
        {
            // Arrange
            using var httpTest = new HttpTest();
            httpTest.RespondWith(SuccessResponse);

            // Act
            var response = await _cnbClient.GetLatestExchangeRatesAsync();

            // Assert
            var expectedRates = new List<ExchangeRate>
                {
                    new ExchangeRate("Australia", "dollar", 1, "AUD", "CZK", 15.751m),
                    new ExchangeRate("Iceland", "krona", 100, "ISK", "CZK", 17.434m),
                    new ExchangeRate("New Zealand", "dollar", 1, "NZD", "CZK", 14.119m),
                };

            var expectedResponse = new DailyExchangeRates(new DateOnly(2022, 10, 21), expectedRates);

            response.Should().BeEquivalentTo(expectedResponse);
        }

        [Test]
        public async Task WhenGetLatestExchangeRatesHasRateFormatErrorsThenOtherRatesAreParsed()
        {
            // Arrange
            using var httpTest = new HttpTest();

            var SuccessResponse = @"21 Oct 2022 #201
Country|Currency|Amount|Code|Rate
Australia|dollar1|AUD|15.751
Iceland|krona|100|ISK|17.434
New Zealand|dollar|1|NZD|14.119g";

        httpTest.RespondWith(SuccessResponse);

            // Act
            var response = await _cnbClient.GetLatestExchangeRatesAsync();

            // Assert
            var expectedRates = new List<ExchangeRate>
                {
                    new ExchangeRate("Iceland", "krona", 100, "ISK", "CZK", 17.434m)
                };

            var expectedResponse = new DailyExchangeRates(new DateOnly(2022, 10, 21), expectedRates);

            response.Should().BeEquivalentTo(expectedResponse);
        }

        [Test]
        public async Task WhenGetLatestExchangeRatesIsRedirectedThenFollowsRedirect()
        {
            // Arrange
            using var httpTest = new HttpTest();
            httpTest.RespondWith("", 302, new { Location = "http://test2.test" });
            httpTest.RespondWith(SuccessResponse);

            // Act
            var response = await _cnbClient.GetLatestExchangeRatesAsync();

            // Assert
            var expectedRates = new List<ExchangeRate>
                {
                    new ExchangeRate("Australia", "dollar", 1, "AUD", "CZK", 15.751m),
                    new ExchangeRate("Iceland", "krona", 100, "ISK", "CZK", 17.434m),
                    new ExchangeRate("New Zealand", "dollar", 1, "NZD", "CZK", 14.119m),
                };

            var expectedResponse = new DailyExchangeRates(new DateOnly(2022, 10, 21), expectedRates);

            response.Should().BeEquivalentTo(expectedResponse);
        }


        [TestCase(404)]
        [TestCase(500)]
        public void WhenGetLatestExchangeRatesFailsThenThrowsException(int responseStatus)
        {
            using var httpTest = new HttpTest();
            httpTest.RespondWith("", responseStatus);

            Assert.ThrowsAsync<FlurlHttpException>(() => _cnbClient.GetLatestExchangeRatesAsync());
        }
    }
}