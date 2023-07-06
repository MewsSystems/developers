using ExchangeEntities;
using ExchangeRateUpdater.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq.AutoMock;
using NUnit.Framework;

namespace ExchangeRateUpdaterTests.Services
{
	public class CurrenciesProviderTests
	{
		private AutoMocker _autoMocker;
        private List<Currency> currencies;

		[SetUp]
		public void SetUp()
        {
			_autoMocker = new AutoMocker();

            currencies = new List<Currency>()
            {
                new Currency("EUR"),
                new Currency("USD")
            };

            var appSettings = new Dictionary<string, string>
            {
                { "Currencies:0", "EUR" },
                { "Currencies:1", "USD" },
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(appSettings)
                .Build();

            _autoMocker.Use<IConfiguration>(configuration);
        }

        [Test]
        public void GetCurrenciesFromConfig_Successfully()
        {
            // Arrange
            var sut = _autoMocker.CreateInstance<CurrenciesProvider>();

            // Act
            var result = sut.GetCurrenciesFromConfig();

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(currencies.Count());
            result.Should().BeEquivalentTo(currencies);
        }
	}
}

