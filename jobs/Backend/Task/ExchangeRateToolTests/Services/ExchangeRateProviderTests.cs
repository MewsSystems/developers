using CnbServiceClient.DTOs;
using CnbServiceClient.Interfaces;
using ExchangeEntities;
using ExchangeRateTool.Interfaces;
using ExchangeRateTool.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;

namespace ExchangeRateToolTests.Services
{
	public class ExchangeRateProviderTests
	{
        private AutoMocker _autoMocker;
        private Mock<IExratesService> _exrateServiceMock;
        private Mock<IExrateFilterService> _exrateFilterServiceMock;
        private Mock<IExchangeRateFactory> _exchangeRateFactoryMock;

        private Exrate Exrate;
        private ExchangeRate ExchangeRate;
        private string Test = "Test";
        private string Test2 = "Test2";
        private decimal Rate = 1;

        [SetUp]
        public void SetUp()
        {
            _autoMocker = new AutoMocker();

            Exrate = new Exrate()
            {
                CurrencyCode = Test,
                Rate = Rate
            };

            ExchangeRate = new ExchangeRate(new Currency(Test), new Currency(Test2), Rate);

            var exrates = new List<Exrate>()
            {
                Exrate,
            };

            _exrateServiceMock = _autoMocker.GetMock<IExratesService>();
            _exrateServiceMock.Setup(x => x.GetExratesDailyAsync())
                .ReturnsAsync(exrates);

            _exrateFilterServiceMock = _autoMocker.GetMock<IExrateFilterService>();
            _exrateFilterServiceMock.Setup(x => x.Filter(It.IsAny<IEnumerable<Exrate>>(), It.IsAny<IEnumerable<Currency>>()))
                .Returns(exrates);

            _exchangeRateFactoryMock = _autoMocker.GetMock<IExchangeRateFactory>();
            _exchangeRateFactoryMock.Setup(x => x.Build(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()))
                .Returns(ExchangeRate);

            var appSettings = new Dictionary<string, string>
            {
                { "TargetCurrencyCode", Test2 }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(appSettings)
                .Build();

            _autoMocker.Use<IConfiguration>(configuration);
        }

        [Test]
        public async Task GetExchangeRatesAsync_Successfully()
        {
            // Arrange
            var currencies = new List<Currency>()
            {
                new Currency(Test)
            };

            var sut = _autoMocker.CreateInstance<ExchangeRateProvider>();

            // Act
            var result = await sut.GetExchangeRatesAsync(currencies);

            // Assert
            result.Should().BeAssignableTo<IEnumerable<ExchangeRate>>();
            result.Count().Should().Be(1);
            result.First().Should().Be(ExchangeRate);
        }
    }
}

