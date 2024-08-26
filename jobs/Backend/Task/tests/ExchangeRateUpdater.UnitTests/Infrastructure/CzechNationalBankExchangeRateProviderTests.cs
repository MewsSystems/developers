using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ExchangeRateUpdater.UnitTests.Infrastructure
{
    public class CzechNationalBankExchangeRateProviderTests
    {
        private Mock<ILogger<CzechNationalBankExchangeRateProvider>> mockedLogger = new Mock<ILogger<CzechNationalBankExchangeRateProvider>>();

        [Fact]
        public async Task GetExchangeRatesAsync_AvailableCzechNationalBankMockedResource_ReturnsUsdAndEurExchangeRates()
        {
            // Arrange
            var exchangeRateProvider = new CzechNationalBankExchangeRateProvider(
                restClient: CzechNationalBankServiceHelper.CreateResponsiveMockedCzechNationalBankService().Object,
                retryPoliciesBuilder: new FakeRetryPoliciesBuilder(),
                logger: mockedLogger.Object);

            var currencies = new[] { CurrenciesHelper.USD, CurrenciesHelper.EUR };

            // Act
            var rates = await exchangeRateProvider.GetExchangeRatesAsync(currencies);

            // Assert
            rates.Should().HaveCount(2);

            rates.Should()
                .Contain(exchangeRate => exchangeRate.SourceCurrency.Code == CurrenciesHelper.USD.Code
                                         && exchangeRate.TargetCurrency.Code == Currency.DEFAULT_CURRENCY.Code
                                         && exchangeRate.Value == CzechNationalBankServiceHelper.USD_RATE);
            rates.Should()
                .Contain(exchangeRate => exchangeRate.SourceCurrency.Code == CurrenciesHelper.EUR.Code
                                         && exchangeRate.TargetCurrency.Code == Currency.DEFAULT_CURRENCY.Code
                                         && exchangeRate.Value == CzechNationalBankServiceHelper.EUR_RATE);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.InternalServerError)]
        public async Task GetExchangeRatesAsync_ErroringCzechNationalBankMockedResource_LogsErrorAndThrowsApplicationException(HttpStatusCode httpStatusCode)
        {
            // Arrange
            var exchangeRateProvider = new CzechNationalBankExchangeRateProvider(
                restClient: CzechNationalBankServiceHelper.CreateErroringMockedCzechNationalBankService(httpStatusCode).Object,
                retryPoliciesBuilder: new FakeRetryPoliciesBuilder(),
                logger: mockedLogger.Object);
            var currencies = new[] { CurrenciesHelper.USD, CurrenciesHelper.EUR };

            var action = async () => await exchangeRateProvider.GetExchangeRatesAsync(currencies);

            // Act + Assert
            await action.Should().ThrowAsync<ApplicationException>();
            
            mockedLogger.Verify(logger => logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                    It.Is<EventId>(eventId => eventId.Id == 0),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Theory, MemberData(nameof(EmptyCurrenciesLists))]
        public async Task GetExchangeRatesAsync_WithEmptyOrNullCurrencies_EarlyReturnsEmptyArrayOfExchangeRates(IEnumerable<Currency> currencies)
        {
            // Arrange
            var responsiveCnbMockedService = CzechNationalBankServiceHelper.CreateResponsiveMockedCzechNationalBankService();
            var exchangeRateProvider = new CzechNationalBankExchangeRateProvider(
                restClient: responsiveCnbMockedService.Object,
                retryPoliciesBuilder: new FakeRetryPoliciesBuilder(),
                logger: mockedLogger.Object);

            // Act
            var rates = await exchangeRateProvider.GetExchangeRatesAsync(currencies);

            // Assert
            rates.Should().HaveCount(0);
            responsiveCnbMockedService.Verify(s => s.ExecuteAsync(It.IsAny<RestRequest>(), default), Times.Never);
        }

        public static IEnumerable<object[]> EmptyCurrenciesLists =>
            new List<object[]>
            {
                new object[] { null },
                new object[] { new Currency[0] }
            };

        [Fact]
        public async Task GetExchangeRatesAsync_TransientErrorsInCzechNationalBankMockedResource_ReturnsUsdAndEurExchangeRatesAfter3Retries()
        {
            // Arrange
            var transientErrorCnbServiceMock = CzechNationalBankServiceHelper.CreateTransientErrorMockedCzechNationalBankService();

            var exchangeRateProvider = new CzechNationalBankExchangeRateProvider(
                restClient: transientErrorCnbServiceMock.Object,
                retryPoliciesBuilder: new FakeRetryPoliciesBuilder(),
                logger: mockedLogger.Object);

            var currencies = new[] { CurrenciesHelper.USD, CurrenciesHelper.EUR };

            // Act
            var rates = await exchangeRateProvider.GetExchangeRatesAsync(currencies);

            // Assert
            transientErrorCnbServiceMock.Verify(s => s.ExecuteAsync(It.IsAny<RestRequest>(), default), Times.Exactly(4));

            rates.Should().HaveCount(2);

            rates.Should()
                .Contain(exchangeRate => exchangeRate.SourceCurrency.Code == CurrenciesHelper.USD.Code
                                         && exchangeRate.TargetCurrency.Code == Currency.DEFAULT_CURRENCY.Code
                                         && exchangeRate.Value == CzechNationalBankServiceHelper.USD_RATE);
            rates.Should()
                .Contain(exchangeRate => exchangeRate.SourceCurrency.Code == CurrenciesHelper.EUR.Code
                                         && exchangeRate.TargetCurrency.Code == Currency.DEFAULT_CURRENCY.Code
                                         && exchangeRate.Value == CzechNationalBankServiceHelper.EUR_RATE);
        }
    }
}
