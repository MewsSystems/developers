using Microsoft.Extensions.Logging;
using ExchangeRateUpdater;
using Moq;
using ExchangeRateUpdater.Helpers;
using ExchangeRateUpdater.Interfaces;
using Moq.Protected;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateUpdaterTests
    {
        private Mock<IApiFetcher> apiFetcherMock;
        private Mock<ILogger<Logger>> loggerMock;

        [SetUp]
        public void Setup()
        {
            loggerMock = new Mock<ILogger<Logger>>();
            apiFetcherMock = new Mock<IApiFetcher>();
        }


        [Test]
        public void ApiResponseNotNull()
        {
            // Test that the api returns a response
            //Arrange
            var apiFetcher = new ApiFetcher(loggerMock.Object);

            //Act
            var response = apiFetcher.GetExchangeRates();

            //Assert
            Assert.That(response, Is.Not.Null);
        }

        [Test]
        public void ApiResponseNull()
        {
            // Test that the api returns a response

            //Arrange
            apiFetcherMock.Setup(x => x.GetExchangeRates()).Returns((ApiResponse)null);

            var exchangeRateProvider = new ExchangeRateProvider(loggerMock.Object, apiFetcherMock.Object);

            IEnumerable<Currency> currencies = new[]
            {
                new Currency("USD"),
                new Currency("EUR"),
                new Currency("CZK"),
                new Currency("JPY"),
                new Currency("KES"),
                new Currency("RUB"),
                new Currency("THB"),
                new Currency("TRY"),
                new Currency("XYZ")
            };

            //Act
            var response = exchangeRateProvider.GetExchangeRates(currencies);

            //Assert
            Assert.That(response, Is.Null);
        }

        [Test]
        public void GetExchangeRates()
        { 
            // Tests that the exchange rates are retrieved and filtered by FilterRates correctly

            //Arrange
            apiFetcherMock.Setup(x => x.GetExchangeRates()).Returns(new ApiResponse
            {
                Rates = new List<ApiResponse.RateObject>()
                {
                    new ApiResponse.RateObject
                    {
                        ISOCode = "USD",
                        Rate = 26.2M
                    },
                    new ApiResponse.RateObject
                    {
                        ISOCode = "EUR",
                        Rate = 23.0M
                    },
                }
            });

            IEnumerable<Currency> currencies = new[]
            {
                new Currency("USD"),
                new Currency("EUR"),
                new Currency("CZK")
            };

            var exchangeRateProvider = new ExchangeRateProvider(loggerMock.Object, apiFetcherMock.Object);

            //Act
            var response = exchangeRateProvider.GetExchangeRates(currencies);

            //Assert
            IEnumerable<ExchangeRate> expected = new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), 26.2M),
                new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 23.0M)
            };

            Assert.That(response, Has.Exactly(2).Items);
            Assert.That(response, Is.EqualTo(expected));
        }

        [Test]
        public void GetExchangeRatesNull()
        {
            //Arrange
            apiFetcherMock.Setup(x => x.GetExchangeRates()).Returns(new ApiResponse
            {
                Rates = new List<ApiResponse.RateObject>()
                {
                    new ApiResponse.RateObject
                    {
                        ISOCode = "USD",
                        Rate = 26.2M
                    },
                    new ApiResponse.RateObject
                    {
                        ISOCode = "EUR",
                        Rate = 23.0M
                    },
                }
            });
            IEnumerable<Currency> currencies = new[]
            {
                new Currency("CZK")
            };
            var exchangeRateProvider = new ExchangeRateProvider(loggerMock.Object, apiFetcherMock.Object);

            //Act
            var response = exchangeRateProvider.GetExchangeRates(currencies);

            //Assert
            Assert.That(response, Is.Empty);
        }
    }
}