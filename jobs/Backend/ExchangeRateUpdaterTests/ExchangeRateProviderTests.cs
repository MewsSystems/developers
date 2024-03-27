using Castle.Components.DictionaryAdapter.Xml;
using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Infrastructure.Service;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Reflection;

namespace ExchangeRateUpdaterTests
{
    public class ExchangeRateProviderTests
    {
        private Mock<IExchangeRateService> _exchangeRateService;
        private Mock<ILogger<IExchangeRateService>> _logger;
        private ExchangeRateProvider _exchangeRateProvider;

        [SetUp]
        public void Setup()
        {
            _exchangeRateService = new Mock<IExchangeRateService>();
            _logger = new Mock<ILogger<IExchangeRateService>>();
            _exchangeRateProvider = new ExchangeRateProvider(_exchangeRateService.Object, _logger.Object);
        }

        [Test]
        public async Task GetExchangeRates_Returns_ExchangeRates()
        {
            //Arrange
            var currencies = GetCurrencyTestData();
            var exchangeRateResponse = GetExchangeRateResponseTestData();
            var exchangeRates = GetExchangeRatesTestData();            

            _exchangeRateService.Setup(x => x.GetDailyExchangeRates(CancellationToken.None)).ReturnsAsync(exchangeRateResponse);

            //Act
            var result = await _exchangeRateProvider.GetExchangeRates(currencies);

            //Assert
            result.ToArray().Should().BeEquivalentTo(exchangeRates);
        }

        [Test]
        public async Task GetExchangeRates_ExchangeRateResponseIsEmpty_ThrowArgumentNullException()
        {
            //Arrange
            var currencies = GetCurrencyTestData();
            var exchangeRateResponse = new ExchangeRateResponse() { };
            var exchangeRates = GetExchangeRatesTestData();

            _exchangeRateService.Setup(x => x.GetDailyExchangeRates(CancellationToken.None)).ReturnsAsync(exchangeRateResponse);

            //Act
            await _exchangeRateProvider.Invoking(x => x.GetExchangeRates(currencies)).Should().ThrowAsync<Exception>();
        }

        [Test]
        public async Task GetExchangeRates_InvalidCurrency_ReturnEmpty()
        {
            //Arrange
            var currencies = new[]
            {
                new Currency("ABC"),
                new Currency("XYZ")
            };
            var exchangeRateResponse = GetExchangeRateResponseTestData();

            _exchangeRateService.Setup(x => x.GetDailyExchangeRates(CancellationToken.None)).ReturnsAsync(exchangeRateResponse);

            //Act
            var result = await _exchangeRateProvider.GetExchangeRates(currencies);

            //Assert
            result.Should().BeEmpty();
        }

        public IEnumerable<Currency> GetCurrencyTestData()
        {
            return new[]
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
        }

        public ExchangeRateResponse GetExchangeRateResponseTestData()
        {
            return new ExchangeRateResponse()
            {
                Rates = new[]
                {
                    new ExchangeRateData() { CurrencyCode = "AUD", Rate = 15.249M },
                    new ExchangeRateData() { CurrencyCode = "BRL", Rate = 4.675M },
                    new ExchangeRateData() { CurrencyCode = "BGN", Rate = 12.922M },
                    new ExchangeRateData() { CurrencyCode = "CAD", Rate = 17.167M },
                    new ExchangeRateData() { CurrencyCode = "CNY", Rate = 3.225M },
                    new ExchangeRateData() { CurrencyCode = "DKK", Rate = 3.389M },
                    new ExchangeRateData() { CurrencyCode = "EUR", Rate = 25.275M },
                    new ExchangeRateData() { CurrencyCode = "HKD", Rate = 2.975M },
                    new ExchangeRateData() { CurrencyCode = "HUF", Rate = 6.385M },
                    new ExchangeRateData() { CurrencyCode = "ISK", Rate = 16.929M },
                    new ExchangeRateData() { CurrencyCode = "XDR", Rate = 30.857M },
                    new ExchangeRateData() { CurrencyCode = "INR", Rate = 27.941M },
                    new ExchangeRateData() { CurrencyCode = "IDR", Rate = 1.474M },
                    new ExchangeRateData() { CurrencyCode = "ILS", Rate = 6.358M },
                    new ExchangeRateData() { CurrencyCode = "JPY", Rate = 15.375M },
                    new ExchangeRateData() { CurrencyCode = "MYR", Rate = 4.932M },
                    new ExchangeRateData() { CurrencyCode = "MXN", Rate = 1.395M },
                    new ExchangeRateData() { CurrencyCode = "NZD", Rate = 14.011M },
                    new ExchangeRateData() { CurrencyCode = "NOK", Rate = 2.173M },
                    new ExchangeRateData() { CurrencyCode = "PHP", Rate = 41.387M },
                    new ExchangeRateData() { CurrencyCode = "PLN", Rate = 5.868M },
                    new ExchangeRateData() { CurrencyCode = "RON", Rate = 5.085M },
                    new ExchangeRateData() { CurrencyCode = "SGD", Rate = 17.317M },
                    new ExchangeRateData() { CurrencyCode = "ZAR", Rate = 1.231M },
                    new ExchangeRateData() { CurrencyCode = "KRW", Rate = 1.735M },
                    new ExchangeRateData() { CurrencyCode = "SEK", Rate = 2.207M },
                    new ExchangeRateData() { CurrencyCode = "CHF", Rate = 25.758M },
                    new ExchangeRateData() { CurrencyCode = "THB", Rate = 64.139M },
                    new ExchangeRateData() { CurrencyCode = "TRY", Rate = 72.288M },
                    new ExchangeRateData() { CurrencyCode = "GBP", Rate = 29.441M },
                    new ExchangeRateData() { CurrencyCode = "USD", Rate = 23.278M }
                }
            };
        }

        public IEnumerable<ExchangeRate> GetExchangeRatesTestData()
        {
            return new[]
            {
                new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 25.275M),
                new ExchangeRate(new Currency("JPY"), new Currency("CZK"), 15.375M),
                new ExchangeRate(new Currency("THB"), new Currency("CZK"), 64.139M),
                new ExchangeRate(new Currency("TRY"), new Currency("CZK"), 72.288M),
                new ExchangeRate(new Currency("USD"), new Currency("CZK"), 23.278M)
            };
        }
    }
}