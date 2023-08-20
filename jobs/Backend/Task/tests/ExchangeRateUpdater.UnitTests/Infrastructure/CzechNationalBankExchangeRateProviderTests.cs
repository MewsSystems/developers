﻿using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
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
                restClient: CzechNationalBankServiceHelper.CreateResponsiveMockedCzechNationalBankService(),
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
                restClient: CzechNationalBankServiceHelper.CreateErroringMockedCzechNationalBankService(httpStatusCode),
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
    }
}
