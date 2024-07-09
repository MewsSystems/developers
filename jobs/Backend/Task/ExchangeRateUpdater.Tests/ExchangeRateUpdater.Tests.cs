using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using System;

namespace ExchangeRateUpdater.Tests
{

    public class ExchangeRateTests
    {
        [Fact]
        public void Constructor_ShouldInitializeExchangeRate_WhenParametersAreValid()
        {
            // Arrange
            var sourceCurrency = new Currency("USD");
            var targetCurrency = new Currency("EUR");
            var value = 0.85m;

            // Act
            var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, value);

            // Assert
            exchangeRate.SourceCurrency.Should().Be(sourceCurrency);
            exchangeRate.TargetCurrency.Should().Be(targetCurrency);
            exchangeRate.Value.Should().Be(value);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenSourceCurrencyIsNull()
        {
            // Arrange
            Currency sourceCurrency = null;
            var targetCurrency = new Currency("EUR");
            var value = 0.85m;

            // Act
            Action act = () => new ExchangeRate(sourceCurrency, targetCurrency, value);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Source currency cannot be null. (Parameter 'sourceCurrency')");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenTargetCurrencyIsNull()
        {
            // Arrange
            var sourceCurrency = new Currency("USD");
            Currency targetCurrency = null;
            var value = 0.85m;

            // Act
            Action act = () => new ExchangeRate(sourceCurrency, targetCurrency, value);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Target currency cannot be null. (Parameter 'targetCurrency')");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenValueIsZero()
        {
            // Arrange
            var sourceCurrency = new Currency("USD");
            var targetCurrency = new Currency("EUR");
            var value = 0m;

            // Act
            Action act = () => new ExchangeRate(sourceCurrency, targetCurrency, value);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("Exchange rate value must be greater than zero. (Parameter 'value')");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentOutOfRangeException_WhenValueIsNegative()
        {
            // Arrange
            var sourceCurrency = new Currency("USD");
            var targetCurrency = new Currency("EUR");
            var value = -1m;

            // Act
            Action act = () => new ExchangeRate(sourceCurrency, targetCurrency, value);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("Exchange rate value must be greater than zero. (Parameter 'value')");
        }

        [Fact]
        public void ToString_ShouldReturnCorrectFormat()
        {
            // Arrange
            var sourceCurrency = new Currency("USD");
            var targetCurrency = new Currency("EUR");
            var value = 0.85m;
            var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, value);

            // Act
            var result = exchangeRate.ToString();

            // Assert
            result.Should().Be("USD/EUR=0.85");
        }
    }

    public class CurrencyTests
    {
        [Fact]
        public void Constructor_ShouldInitializeCurrency_WhenCodeIsValid()
        {
            // Arrange
            var validCode = "USD";

            // Act
            var currency = new Currency(validCode);

            // Assert
            currency.Code.Should().Be(validCode);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenCodeIsNull()
        {
            // Act
            Action act = () => new Currency(null);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Currency code cannot be null or empty. (Parameter 'code')");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenCodeIsEmpty()
        {
            // Act
            Action act = () => new Currency(string.Empty);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Currency code cannot be null or empty. (Parameter 'code')");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenCodeIsWhitespace()
        {
            // Act
            Action act = () => new Currency("   ");

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Currency code cannot be null or empty. (Parameter 'code')");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenCodeIsTooShort()
        {
            // Act
            Action act = () => new Currency("US");

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Currency code must be a three-letter ISO 4217 code consisting of uppercase letters. (Parameter 'code')");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenCodeIsTooLong()
        {
            // Act
            Action act = () => new Currency("USDE");

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Currency code must be a three-letter ISO 4217 code consisting of uppercase letters. (Parameter 'code')");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenCodeIsLowercase()
        {
            // Act
            Action act = () => new Currency("usd");

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("Currency code must be a three-letter ISO 4217 code consisting of uppercase letters. (Parameter 'code')");
        }

        [Fact]
        public void ToString_ShouldReturnCode()
        {
            // Arrange
            var validCode = "USD";
            var currency = new Currency(validCode);

            // Act
            var result = currency.ToString();

            // Assert
            result.Should().Be(validCode);
        }
    }
    public class ExchangeRateProviderTests
    {
        [Fact]
        public async Task GetExchangeRatesAsync_ShouldReturnExchangeRates_ForSpecifiedCurrencies()
        {
            // Arrange
            var mockExchangeRateService = new Mock<IExchangeRateService>();
            mockExchangeRateService
                .Setup(service => service.FetchExchangeRateDataAsync())
                .ReturnsAsync("31 Jan 2022 #21\nCountry|Currency|Amount|Code|Rate\nUSA|dollar|1|USD|21.657\nEurozone|euro|1|EUR|24.865\nUK|pound|1|GBP|29.752");

            var provider = new CnbExchangeRateProvider(mockExchangeRateService.Object);
            var currencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("EUR"),
                new Currency("GBP")
            };

            // Act
            var exchangeRates = await provider.GetExchangeRatesAsync(currencies);

            // Assert
            Assert.Equal(3, exchangeRates.Count());
            Assert.Contains(exchangeRates, rate => rate.TargetCurrency.Code == "USD" && rate.Value == 21.657m);
            Assert.Contains(exchangeRates, rate => rate.TargetCurrency.Code == "EUR" && rate.Value == 24.865m);
            Assert.Contains(exchangeRates, rate => rate.TargetCurrency.Code == "GBP" && rate.Value == 29.752m);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ShouldIgnoreNonSpecifiedCurrencies()
        {
            // Arrange
            var mockExchangeRateService = new Mock<IExchangeRateService>();
            mockExchangeRateService
                .Setup(service => service.FetchExchangeRateDataAsync())
                .ReturnsAsync("31 Jan 2022 #21\nCountry|Currency|Amount|Code|Rate\nUSA|dollar|1|USD|21.657\nEurozone|euro|1|EUR|24.865\nUK|pound|1|GBP|29.752");

            var provider = new CnbExchangeRateProvider(mockExchangeRateService.Object);
            var currencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("EUR")
            };

            // Act
            var exchangeRates = await provider.GetExchangeRatesAsync(currencies);

            // Assert
            Assert.Equal(2, exchangeRates.Count());
            Assert.Contains(exchangeRates, rate => rate.TargetCurrency.Code == "USD" && rate.Value == 21.657m);
            Assert.Contains(exchangeRates, rate => rate.TargetCurrency.Code == "EUR" && rate.Value == 24.865m);
            Assert.DoesNotContain(exchangeRates, rate => rate.TargetCurrency.Code == "GBP");
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ShouldReturnEmpty_ForNonExistingCurrencies()
        {
            // Arrange
            var mockExchangeRateService = new Mock<IExchangeRateService>();
            mockExchangeRateService
                .Setup(service => service.FetchExchangeRateDataAsync())
                .ReturnsAsync("31 Jan 2022 #21\nCountry|Currency|Amount|Code|Rate\nUSA|dollar|1|USD|21.657\nEurozone|euro|1|EUR|24.865\nUK|pound|1|GBP|29.752");

            var provider = new CnbExchangeRateProvider(mockExchangeRateService.Object);
            var currencies = new List<Currency>
            {
                new Currency("JPY"),
                new Currency("AUD")
            };

            // Act
            var exchangeRates = await provider.GetExchangeRatesAsync(currencies);

            // Assert
            Assert.Empty(exchangeRates);
        }
    }
}
