using ExchangeRateUpdater.Application.DTOs;
using ExchangeRateUpdater.Application.Providers;
using ExchangeRateUpdater.Core.Entities;
using ExchangeRateUpdater.Core.Exceptions;
using ExchangeRateUpdater.Data;
using ExchangeRateUpdater.Infrastructure.Sources;
using ExchangeRateUpdater.Tests.UnitTests.Data;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ExchangeRateUpdater.Tests.UnitTests
{
    public class ExchangeRateProviderTests
    {
        private readonly Mock<IExchangeRateSource> _sourceMock;
        private readonly Mock<ILogger<ExchangeRateProvider>> _loggerMock;

        public ExchangeRateProviderTests()
        {
            _sourceMock = new Mock<IExchangeRateSource>();
            _loggerMock = new Mock<ILogger<ExchangeRateProvider>>();
        }

        [Fact]
        public async Task GetExchangeRates_ReturnsOnlyRatesForRequestedCurrencies()
        {
            // Arrange
            _sourceMock.Setup(s => s.GetDailyExchangeRates(null)).ReturnsAsync(ExchangeRateProviderTestData.ValidRates);

            var provider = new ExchangeRateProvider(_loggerMock.Object, _sourceMock.Object);

            var currencies = new[] { new Currency("USD") };

            // Act
            var result = (await provider.GetExchangeRates(currencies)).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal("USD", result[0].TargetCurrency.Code);
        }

        [Fact]
        public async Task GetExchangeRates_IgnoresRequestedCurrenciesNotInSource()
        {
            // Arrange
            _sourceMock.Setup(s => s.GetDailyExchangeRates(null)).ReturnsAsync(ExchangeRateProviderTestData.ValidRates);

            var provider = new ExchangeRateProvider(_loggerMock.Object, _sourceMock.Object);

            var currencies = new[]
            {
                new Currency("USD"),
                new Currency("GBP")
            };

            // Act
            var result = (await provider.GetExchangeRates(currencies)).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal("USD", result[0].TargetCurrency.Code);
        }

        [Fact]
        public async Task GetExchangeRates_SkipsInvalidExchangeRateDTOs()
        {
            // Arrange
            _sourceMock.Setup(s => s.GetDailyExchangeRates(null)).ReturnsAsync(ExchangeRateProviderTestData.WithInvalidRate);

            var provider = new ExchangeRateProvider(_loggerMock.Object, _sourceMock.Object);

            var currencies = new[]
            {
                new Currency("USD"),
                new Currency("EUR")
            };

            // Act
            var result = (await provider.GetExchangeRates(currencies)).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal("EUR", result[0].TargetCurrency.Code);
        }

        [Fact]
        public async Task GetExchangeRates_NormalizesRate_WhenAmountIsGreaterThanOne()
        {
            // Arrange
            _sourceMock.Setup(s => s.GetDailyExchangeRates(null)).ReturnsAsync(ExchangeRateProviderTestData.WithAmountGreaterThanOne);

            var provider = new ExchangeRateProvider(_loggerMock.Object, _sourceMock.Object);

            // Act
            var result = (await provider.GetExchangeRates(new[] { new Currency("USD") })).Single();

            // Assert
            Assert.Equal(22m, result.Value);
        }

        [Fact]
        public async Task GetExchangeRates_SkipsNullExchangeRateDTOs()
        {
            // Arrange
            _sourceMock.Setup(s => s.GetDailyExchangeRates(null)).ReturnsAsync(ExchangeRateProviderTestData.WithNullAndInvalidEntries);

            var provider = new ExchangeRateProvider(_loggerMock.Object, _sourceMock.Object);

            // Act
            var result = (await provider.GetExchangeRates(new[] { new Currency("USD") })).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal("USD", result[0].TargetCurrency.Code);
        }

        [Fact]
        public async Task GetExchangeRates_SkipsBaseCurrencyDTOs()
        {
            // Arrange
            _sourceMock.Setup(s => s.GetDailyExchangeRates(null)).ReturnsAsync(ExchangeRateProviderTestData.WithBaseCurrency);

            var provider = new ExchangeRateProvider(_loggerMock.Object, _sourceMock.Object);

            var currencies = new[]
            {
                new Currency("USD"),
                new Currency("CZK"),
                new Currency("EUR")
            };

            // Act
            var result = (await provider.GetExchangeRates(currencies)).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal("USD", result[0].TargetCurrency.Code);
        }

        [Fact]
        public async Task GetExchangeRates_Throws_WhenSourceThrows()
        {
            // Arrange
            _sourceMock.Setup(s => s.GetDailyExchangeRates(null)).ThrowsAsync(new ExchangeRateSourceException("Failure"));

            var provider = new ExchangeRateProvider(_loggerMock.Object, _sourceMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ExchangeRateSourceException>(() => provider.GetExchangeRates(new[] { new Currency("USD") }));
        }

        //GetExchangeRatesFromDay

        [Fact]
        public async Task GetExchangeRatesFromDay_ReturnsRatesForGivenDate()
        {
            // Arrange
            var testDate = "2020-02-12";

            _sourceMock.Setup(s => s.GetDailyExchangeRates(testDate)).ReturnsAsync(ExchangeRateProviderTestData.ValidRates);

            var provider = new ExchangeRateProvider(_loggerMock.Object, _sourceMock.Object);

            var currencies = new[] { new Currency("USD") };

            // Act
            var result = (await provider.GetExchangeRatesFromDay(currencies, testDate)).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal("USD", result[0].TargetCurrency.Code);
        }

        [Fact]
        public async Task GetExchangeRatesFromDay_Throws_WhenDateFormatInvalid()
        {
            // Arrange
            var provider = new ExchangeRateProvider(_loggerMock.Object, _sourceMock.Object);
            string invalidDate = "2020/01/01"; // wrong format

            // Act & Assert
            // Date format itself is not validated in provider, so mocking source to throw
            _sourceMock.Setup(s => s.GetDailyExchangeRates(invalidDate)).ThrowsAsync(new ExchangeRateSourceException("Invalid date format"));

            await Assert.ThrowsAsync<ExchangeRateSourceException>(() =>
                provider.GetExchangeRatesFromDay(new[] { new Currency("USD") }, invalidDate));
        }

        [Fact]
        public async Task GetExchangeRatesFromDay_Throws_WhenDateInFuture()
        {
            // Arrange
            var provider = new ExchangeRateProvider(_loggerMock.Object, _sourceMock.Object);
            string futureDate = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");

            // Act & Assert
            _sourceMock.Setup(s => s.GetDailyExchangeRates(futureDate))
                .ThrowsAsync(new ExchangeRateSourceException("Date cannot be in the future"));

            await Assert.ThrowsAsync<ExchangeRateSourceException>(() =>
                provider.GetExchangeRatesFromDay(new[] { new Currency("USD") }, futureDate));
        }

        [Fact]
        public async Task GetExchangeRatesFromDay_ReturnsEmpty_WhenNoDataForTooOldDate()
        {
            // Arrange
            var provider = new ExchangeRateProvider(_loggerMock.Object, _sourceMock.Object);
            string oldDate = "1900-01-01";

            _sourceMock.Setup(s => s.GetDailyExchangeRates(oldDate)).ReturnsAsync(new List<ExchangeRateDTO>()); // No data

            // Act
            var result = (await provider.GetExchangeRatesFromDay(new[] { new Currency("USD") }, oldDate)).ToList();

            // Assert
            Assert.Empty(result);
        }

        //CompareExchangeRatesBetweenDates

        [Fact]
        public async Task CompareExchangeRatesBetweenDates_ReturnsDifferences_ForMatchingCurrencies()
        {
            // Arrange
            var dateA = "2020-01-01";
            var dateB = "2020-01-02";

            _sourceMock.Setup(s => s.GetDailyExchangeRates(dateA)).ReturnsAsync(ExchangeRateProviderTestData.ValidRates);
            _sourceMock.Setup(s => s.GetDailyExchangeRates(dateB)).ReturnsAsync(ExchangeRateProviderTestData.ValidRates);

            var provider = new ExchangeRateProvider(_loggerMock.Object, _sourceMock.Object);

            var currencies = new[]
            {
                new Currency("USD"),
                new Currency("EUR")
            };

            // Act
            var result = (await provider.CompareExchangeRatesBetweenDates(currencies, dateA, dateB)).ToList();

            // Assert
            Assert.Equal(2, result.Count);

            var usd = result.Single(r => r.TargetCurrency.Code == "USD");
            Assert.Equal(22, usd.FirstDateRate);
            Assert.Equal(22, usd.SecondDateRate);
            Assert.Equal(0, usd.Difference);
        }

        [Fact]
        public async Task CompareExchangeRatesBetweenDates_ReturnsDifferences_ForMatchingCurrencies_WithRateDifference()
        {
            // Arrange
            var dateA = "2015-01-01";
            var dateB = "2020-01-01";

            _sourceMock.Setup(s => s.GetDailyExchangeRates(dateA)).ReturnsAsync(ExchangeRateProviderTestData.ValidRates);
            _sourceMock.Setup(s => s.GetDailyExchangeRates(dateB)).ReturnsAsync(ExchangeRateProviderTestData.ValidRatesGreater);

            var provider = new ExchangeRateProvider(_loggerMock.Object, _sourceMock.Object);

            var currencies = new[]
            {
                new Currency("USD"),
                new Currency("EUR")
            };

            // Act
            var result = (await provider.CompareExchangeRatesBetweenDates(currencies, dateA, dateB)).ToList();

            // Assert
            Assert.Equal(2, result.Count);

            var usd = result.Single(r => r.TargetCurrency.Code == "USD");
            Assert.Equal(22, usd.FirstDateRate);
            Assert.Equal(30, usd.SecondDateRate);
            Assert.Equal(8, usd.Difference);
            Assert.Equal(Math.Round((30m - 22m) / 22m * 100, 2), usd.PercentageDifference);

            var eur = result.Single(r => r.TargetCurrency.Code == "EUR");
            Assert.Equal(25, eur.FirstDateRate);
            Assert.Equal(32, eur.SecondDateRate);
            Assert.Equal(7, eur.Difference);
            Assert.Equal(Math.Round((32m - 25m) / 25m * 100, 2), eur.PercentageDifference);
        }

        [Fact]
        public async Task CompareExchangeRatesBetweenDates_ReturnsDifferences_ForMatchingCurrencies_WithNegativeRateDifference()
        {
            // Arrange
            var dateA = "2020-01-01";
            var dateB = "2015-01-01";

            _sourceMock.Setup(s => s.GetDailyExchangeRates(dateA)).ReturnsAsync(ExchangeRateProviderTestData.ValidRatesGreater);
            _sourceMock.Setup(s => s.GetDailyExchangeRates(dateB)).ReturnsAsync(ExchangeRateProviderTestData.ValidRates);

            var provider = new ExchangeRateProvider(_loggerMock.Object, _sourceMock.Object);

            var currencies = new[]
            {
                new Currency("USD"),
                new Currency("EUR")
            };

            // Act
            var result = (await provider.CompareExchangeRatesBetweenDates(currencies, dateA, dateB)).ToList();

            // Assert
            Assert.Equal(2, result.Count);

            var usd = result.Single(r => r.TargetCurrency.Code == "USD");
            Assert.Equal(30, usd.FirstDateRate);
            Assert.Equal(22, usd.SecondDateRate);
            Assert.Equal(-8, usd.Difference);
            Assert.Equal(Math.Round((22m - 30m) / 30m * 100, 2), usd.PercentageDifference);

            var eur = result.Single(r => r.TargetCurrency.Code == "EUR");
            Assert.Equal(32, eur.FirstDateRate);
            Assert.Equal(25, eur.SecondDateRate);
            Assert.Equal(-7, eur.Difference);
            Assert.Equal(Math.Round((25m - 32m) / 32m * 100, 2), eur.PercentageDifference);
        }

        [Fact]
        public async Task CompareExchangeRatesBetweenDates_ReturnsEmpty_WhenFirstDateHasNoData()
        {
            // Arrange
            var dateA = "2020-01-01";
            var dateB = "2020-01-02";

            _sourceMock.Setup(s => s.GetDailyExchangeRates(dateA)).ReturnsAsync(new List<ExchangeRateDTO>());

            _sourceMock.Setup(s => s.GetDailyExchangeRates(dateB)).ReturnsAsync(ExchangeRateProviderTestData.ValidRates);

            var provider = new ExchangeRateProvider(_loggerMock.Object, _sourceMock.Object);

            // Act
            var result = await provider.CompareExchangeRatesBetweenDates(SupportedCurrencies.All, dateA, dateB);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task CompareExchangeRatesBetweenDates_ReturnsEmpty_WhenSecondDateHasNoData()
        {
            // Arrange
            var dateA = "2020-01-01";
            var dateB = "2020-01-02";

            _sourceMock.Setup(s => s.GetDailyExchangeRates(dateA)).ReturnsAsync(ExchangeRateProviderTestData.ValidRates);

            _sourceMock.Setup(s => s.GetDailyExchangeRates(dateB)).ReturnsAsync(new List<ExchangeRateDTO>());

            var provider = new ExchangeRateProvider(_loggerMock.Object, _sourceMock.Object);

            // Act
            var result = await provider.CompareExchangeRatesBetweenDates(SupportedCurrencies.All, dateA, dateB);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task CompareExchangeRatesBetweenDates_SkipsInvalidRates()
        {
            // Arrange
            var dateA = "2020-01-01";
            var dateB = "2020-01-02";

            _sourceMock.Setup(s => s.GetDailyExchangeRates(dateA)).ReturnsAsync(ExchangeRateProviderTestData.WithInvalidRate);
            _sourceMock.Setup(s => s.GetDailyExchangeRates(dateB)).ReturnsAsync(ExchangeRateProviderTestData.ValidRates);

            var provider = new ExchangeRateProvider(_loggerMock.Object, _sourceMock.Object);

            var currencies = new[]
            {
                new Currency("USD"),
                new Currency("EUR")
            };

            // Act
            var result = (await provider.CompareExchangeRatesBetweenDates(currencies, dateA, dateB)).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal("EUR", result[0].TargetCurrency.Code);
        }

        [Fact]
        public async Task CompareExchangeRatesBetweenDates_NormalizesRates_WhenAmountGreaterThanOne()
        {
            // Arrange
            var dateA = "2020-01-01";
            var dateB = "2020-01-02";

            _sourceMock.Setup(s => s.GetDailyExchangeRates(dateA)).ReturnsAsync(ExchangeRateProviderTestData.WithAmountGreaterThanOne);
            _sourceMock.Setup(s => s.GetDailyExchangeRates(dateB)).ReturnsAsync(ExchangeRateProviderTestData.ValidRates);

            var provider = new ExchangeRateProvider(_loggerMock.Object, _sourceMock.Object);

            var currencies = new[] { new Currency("USD") };

            // Act
            var result = (await provider.CompareExchangeRatesBetweenDates(currencies, dateA, dateB)).Single();

            // Assert
            Assert.Equal(22, result.FirstDateRate);
            Assert.Equal(22, result.SecondDateRate);
            Assert.Equal(0, result.Difference);
        }

        [Fact]
        public async Task CompareExchangeRatesBetweenDates_SkipsBaseCurrency()
        {
            // Arrange
            var dateA = "2020-01-01";
            var dateB = "2020-01-02";

            _sourceMock.Setup(s => s.GetDailyExchangeRates(dateA)).ReturnsAsync(ExchangeRateProviderTestData.WithBaseCurrency);
            _sourceMock.Setup(s => s.GetDailyExchangeRates(dateB)).ReturnsAsync(ExchangeRateProviderTestData.WithBaseCurrency);

            var provider = new ExchangeRateProvider(_loggerMock.Object, _sourceMock.Object);

            var currencies = new[]
            {
                new Currency("CZK"),
                new Currency("USD")
            };

            // Act
            var result = (await provider.CompareExchangeRatesBetweenDates(currencies, dateA, dateB)).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal("USD", result[0].TargetCurrency.Code);
        }

        [Fact]
        public async Task CompareExchangeRatesBetweenDates_Throws_WhenSourceThrows()
        {
            // Arrange
            var dateA = "2020-01-01";
            var dateB = "2020-01-02";

            _sourceMock.Setup(s => s.GetDailyExchangeRates(dateA)).ThrowsAsync(new ExchangeRateSourceException("Failure"));

            var provider = new ExchangeRateProvider(_loggerMock.Object, _sourceMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ExchangeRateSourceException>(() => provider.CompareExchangeRatesBetweenDates(new[] { new Currency("USD") }, dateA, dateB));
        }
    }
}
