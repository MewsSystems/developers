using CnbServiceClient.DTOs;
using ExchangeEntities;
using ExchangeRateTool.Services;
using FluentAssertions;
using Moq.AutoMock;
using NUnit.Framework;

namespace ExchangeRateToolTests.Services
{
	public class ExrateFilterServiceTests
	{
        private AutoMocker _autoMocker;

		private Exrate usdExrate;
		private Exrate eurExrate;
		private List<Exrate> exrates;
		private Currency eurCurrency;
		private Currency czkCurrency;

        [SetUp]
		public void SetUp()
		{
			_autoMocker = new AutoMocker();

			usdExrate = new Exrate() { CurrencyCode = "USD" };
			eurExrate = new Exrate() { CurrencyCode = "EUR" };

			exrates = new List<Exrate>()
			{
				usdExrate,
				eurExrate
			};

			eurCurrency = new Currency("EUR");
			czkCurrency = new Currency("CZK");
        }

        [Test]
		public void Filter_Returns_Exrates_Succesfully()
		{
			// Arrange
			var currecies = new List<Currency>()
			{
				eurCurrency
			};

			var sut = _autoMocker.CreateInstance<ExrateFilterService>();

			// Act
			var result = sut.Filter(exrates, currecies);

			// Assert
			result.Should().BeAssignableTo<IEnumerable<Exrate>>();
			result.Count().Should().Be(1);
			result.First().Should().Be(eurExrate);
		}

        [Test]
        public void Filter_Returns_Empty()
        {
            // Arrange
            var currecies = new List<Currency>()
            {
                czkCurrency
            };

            var sut = _autoMocker.CreateInstance<ExrateFilterService>();

            // Act
            var result = sut.Filter(exrates, currecies);

            // Assert
            result.Should().BeAssignableTo<IEnumerable<Exrate>>();
            result.Count().Should().Be(0);
        }
    }
}

