using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRate.Application.DTOs;
using ExchangeRate.Application.Services;
using ExchangeRate.Application.Services.Interfaces;
using ExchangeRate.Infrastructure.ExternalServices.CzechNationalBank;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace ExchangeRate.UnitTest.Services
{
    public class ExchangeRateServiceTest
    {
        private readonly ICzechNationalBankService _cnbService;
        private readonly IParserService _parserService;
        private readonly ILogger<ExchangeRateService> _logger;
        private readonly ExchangeRateService _service;

        public ExchangeRateServiceTest()
        {
            _cnbService = Substitute.For<ICzechNationalBankService>();
            _parserService = Substitute.For<IParserService>();
            _logger = Substitute.For<ILogger<ExchangeRateService>>();
            _service = new ExchangeRateService(_cnbService, _parserService, _logger);
        }

        [Fact]
        public async Task GetDailyExchangeRates_ShouldReturnRates_WhenDataIsAvailable()
        {
            // Arrange
            var expectedData = "Sample data XML";
            var expectedRates = new List<ExchangeRateBankDTO>
            {
                new ExchangeRateBankDTO { Country = "United States", Currency = "Dollar", Amount = 1, Code = "USD", Rate = 23.45m }
            };

            _cnbService.GetDailyExchangeRates()!.Returns(Task.FromResult(expectedData));
            _parserService.ExchangeRateParseXml(expectedData).Returns(expectedRates);

            // Act
            var result = await _service.GetDailyExchangeRates();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.ExchangeRates);
            Assert.Equal("USD", result.ExchangeRates[0].Code);
            Assert.Equal(23.45m, result.ExchangeRates[0].Rate);
        }

        [Fact]
        public async Task GetDailyExchangeRates_ShouldThrowInvalidOperationException_WhenDataIsEmpty()
        {
            // Arrange
            _cnbService.GetDailyExchangeRates().Returns(Task.FromResult<string?>(null));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.GetDailyExchangeRates());
            Assert.Equal("Exchange rate data is empty or null", exception.Message);
        }

        [Fact]
        public async Task GetExchangeRatesByDate_ShouldReturnRates_WhenDataIsAvailable()
        {
            // Arrange
            var date = new DateTime(2023, 10, 10);
            var expectedData = "Sample data Text";
            var expectedRates = new List<ExchangeRateBankDTO>
            {
                new ExchangeRateBankDTO { Country = "United States", Currency = "Dollar", Amount = 1, Code = "USD", Rate = 23.45m }
            };

            _cnbService.GetExchangeRatesByDay(date).Returns(Task.FromResult(expectedData));
            _parserService.ExchangeRateParseText(expectedData).Returns(expectedRates);

            // Act
            var result = await _service.GetExchangeRatesByDate(date);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.ExchangeRates);
            Assert.Equal("USD", result.ExchangeRates[0].Code);
            Assert.Equal(23.45m, result.ExchangeRates[0].Rate);
        }

        [Fact]
        public async Task GetExchangeRatesByDate_ShouldThrowInvalidOperationException_WhenDataIsEmpty()
        {
            // Arrange
            var date = new DateTime(2023, 10, 10);
            _cnbService.GetExchangeRatesByDay(date).Returns(Task.FromResult<string?>(null));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.GetExchangeRatesByDate(date));
            Assert.Equal("Exchange rate data is empty or null", exception.Message);
        }

        [Fact]
        public void GetCurrenciesBank_ShouldReturnCurrencies_WhenRatesAreAvailable()
        {
            // Arrange
            var rates = new List<ExchangeRateBankDTO>
            {
                new ExchangeRateBankDTO { Country = "United States", Currency = "Dollar", Amount = 1, Code = "USD", Rate = 23.45m }
            };
            var expectedCurrencies = new List<CurrencyDTO>
            {
                new CurrencyDTO("USD")
            };

            _parserService.CurrencyParse(rates).Returns(expectedCurrencies);

            // Act
            var currencies = _service.GetCurrenciesBank(new ExchangeRatesBankDTO(rates));

            // Assert
            Assert.NotNull(currencies);
            Assert.Single(currencies.ToList());
            Assert.Equal("USD", currencies.ToList()[0].Code);
        }
    }
}
