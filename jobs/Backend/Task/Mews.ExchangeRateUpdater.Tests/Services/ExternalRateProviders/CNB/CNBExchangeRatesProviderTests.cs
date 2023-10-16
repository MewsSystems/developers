using Mews.ExchangeRateUpdater.Services.ExternalRateProviders.CNB;
using Mews.ExchangeRateUpdater.Services.ExternalRateProviders.CNB.Mapping;
using Mews.ExchangeRateUpdater.Services.Infrastructure;
using Mews.ExchangeRateUpdater.Services.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Net;

namespace Mews.ExchangeRateUpdater.Tests.Services.ExternalRateProviders.CNB
{
    [TestFixture]
    public class CNBExchangeRatesProviderTests
    {
        private Mock<IRestClient> _restClientMock;

        private Mock<IConfiguration> _configurationMock;

        private ExchangeRates _exchangeRates;

        private CNBExchangeRatesProvider _sut;

        [SetUp]
        public void SetUp()
        {
            _restClientMock = new Mock<IRestClient>();
            var httpResponseMessageMock = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("Mocked API response")
            };
            _exchangeRates = new ExchangeRates
            {
                Rates = new List<ExchangeRateDetails>
                {
                    new ExchangeRateDetails
                    {
                        ValidFor = "2023-10-14",
                        Country = "United Kingdom",
                        Amount = 1,
                        Currency = "pound",
                        CurrencyCode = "GBP",
                        Rate = 28.55M
                    },
                    new ExchangeRateDetails
                    {
                        ValidFor = "2023-10-14",
                        Country = "USA",
                        Amount = 1,
                        Currency = "dollar",
                        CurrencyCode = "USD",
                        Rate = 23.44M
                    },
                    new ExchangeRateDetails
                    {
                        ValidFor = "2023-10-14",
                        Country = "India",
                        Amount = 100,
                        Currency = "rupee",
                        CurrencyCode = "INR",
                        Rate = 28.15M
                    }
                }
            };
            _restClientMock.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(httpResponseMessageMock);
            _restClientMock.Setup(x => x.ReadResponse<ExchangeRates>(It.IsAny<HttpResponseMessage>())).ReturnsAsync(_exchangeRates);

            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(x => x["BankApiBaseUrl"]).Returns("MockedBaseUrl");
            _configurationMock.Setup(x => x["RetryCount"]).Returns("3");
            _configurationMock.Setup(x => x["RetryIntervalInSeconds"]).Returns("5");

            _sut = new CNBExchangeRatesProvider(_restClientMock.Object, _configurationMock.Object);
        }

        [Test]
        public async Task GetExchangeRates_OnSuccess_CallsGetOnRestClient()
        {
            // Arrange

            //Act
            await _sut.GetExchangeRates();

            // Assert
            _restClientMock.Verify(x => x.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
        }

        [Test]
        public async Task GetExchangeRates_OnSuccess_CallsReadResponseOnRestClient()
        {
            // Arrange

            //Act
            await _sut.GetExchangeRates();

            // Assert
            _restClientMock.Verify(x => x.ReadResponse<ExchangeRates>(It.IsAny<HttpResponseMessage>()), Times.Once);
        }

        [Test]
        public async Task GetExchangeRates_WhenExceptionInCallingGet_ThrowsException()
        {
            // Arrange
            var httpResponseMessageMock = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent("Mocked API error response")
            };

            var apiErrorResponse = new ErrorResponse
            {
                Description = "ValidationErrorCode: typeMismatch;  Field: lang; Value: ENB",
                ErrorCode = "VALIDATION_ERROR",
                HappenedAt = "2023-10-14 02:35:47",
                EndPoint = "/cnbapi/exrates/daily?lang=ENB"
            };

            _restClientMock.Reset();
            _restClientMock.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(httpResponseMessageMock);
            _restClientMock.Setup(x => x.ReadResponse<ErrorResponse>(It.IsAny<HttpResponseMessage>())).ReturnsAsync(apiErrorResponse);

            //Act
            var exception = Assert.ThrowsAsync<Exception>(async () => await _sut.GetExchangeRates());

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Bad Request"));
        }

        [Test]
        public async Task GetExchangeRates_WhenExceptionInCallingGet_CallsReadResponseOnRestClient()
        {
            // Arrange
            var httpResponseMessageMock = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent("Mocked API error response")
            };

            var apiErrorResponse = new ErrorResponse
            {
                Description = "ValidationErrorCode: typeMismatch;  Field: lang; Value: ENB",
                ErrorCode = "VALIDATION_ERROR",
                HappenedAt = "2023-10-14 02:35:47",
                EndPoint = "/cnbapi/exrates/daily?lang=ENB"
            };

            _restClientMock.Reset();
            _restClientMock.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(httpResponseMessageMock);
            _restClientMock.Setup(x => x.ReadResponse<ErrorResponse>(It.IsAny<HttpResponseMessage>())).ReturnsAsync(apiErrorResponse);

            //Act
            var exception = Assert.ThrowsAsync<Exception>(async () => await _sut.GetExchangeRates());

            // Assert
            _restClientMock.Verify(x => x.ReadResponse<ErrorResponse>(It.IsAny<HttpResponseMessage>()), Times.Once);
        }

        [Test]
        public async Task GetExchangeRates_OnSuccess_ReturnsExchangeRateModelCollection()
        {
            // Arrange

            //Act
            var actual = await _sut.GetExchangeRates();

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.InstanceOf<IEnumerable<ExchangeRateModel>>());
            Assert.That(actual.Count(), Is.EqualTo(_exchangeRates.Rates.Count()));
            foreach (var exchangeRateDetails in _exchangeRates.Rates)
            {
                var actualExchangeRateModel = actual.First(x => x.SourceCurrency.Code == exchangeRateDetails.CurrencyCode);
                Assert.Multiple(() =>
                {
                    Assert.That(actualExchangeRateModel.ValidFor, Is.EqualTo(exchangeRateDetails.ValidFor));
                    Assert.That(actualExchangeRateModel.Country, Is.EqualTo(exchangeRateDetails.Country));
                    Assert.That(actualExchangeRateModel.SourceCurrency.Currency, Is.EqualTo(exchangeRateDetails.Currency));
                    Assert.That(actualExchangeRateModel.SourceCurrency.Code, Is.EqualTo(exchangeRateDetails.CurrencyCode));
                    Assert.That(actualExchangeRateModel.TargetCurrency.Currency, Is.EqualTo("koruna"));
                    Assert.That(actualExchangeRateModel.TargetCurrency.Code, Is.EqualTo("CZK"));
                    Assert.That(actualExchangeRateModel.Amount, Is.EqualTo(exchangeRateDetails.Amount));
                    Assert.That(actualExchangeRateModel.Rate, Is.EqualTo(exchangeRateDetails.Rate));
                });
            }
        }
    }
}
