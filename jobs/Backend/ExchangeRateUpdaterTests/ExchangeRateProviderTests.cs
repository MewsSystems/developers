using ExchangeRateUpdater;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using FluentAssertions;
using Moq;

namespace ExchangeRateUpdaterTests
{
	[TestClass]
	public class ExchangeRateProviderTests
	{
		private Mock<IWebRequestService> mockWebRequestService;
		private Mock<IDataStringParser<IEnumerable<ExchangeRate>>> mockDataStringParser;
		private IExchangeRateProvider exchangeRateProvider;

		[TestInitialize]
		public void TestInitialize()
		{
			mockWebRequestService = new Mock<IWebRequestService>();
			mockDataStringParser = new Mock<IDataStringParser<IEnumerable<ExchangeRate>>>(MockBehavior.Strict);
			exchangeRateProvider = new ExchangeRateProvider(mockWebRequestService.Object, mockDataStringParser.Object);
		}

		[TestMethod]
		public async Task GetExchangeRatesAsync_ReturnsOnlyTheRatesAvailableInTheDataSource()
		{
			var currencies = new List<Currency>
			{
				new Currency("ABC"),
				new Currency("DEF"),
				new Currency("GHI")
			};

			mockDataStringParser
				.Setup(m => m.Parse(It.IsAny<string>()))
				.Returns(new[]
				{
					new ExchangeRate(new Currency("ABC"), new Currency("XYZ"), 1.23M),
					new ExchangeRate(new Currency("GHI"), new Currency("XYZ"), 6.78M)
				});

			var actualExchangeRates = await exchangeRateProvider.GetExchangeRatesAsync(currencies).ConfigureAwait(false);

			actualExchangeRates.ToList().Should()
				.BeEquivalentTo(new[]
				{
					new ExchangeRate(new Currency("ABC"), new Currency("XYZ"), 1.23M),
					new ExchangeRate(new Currency("GHI"), new Currency("XYZ"), 6.78M)
				}, options => options.WithStrictOrdering());
		}

		[TestMethod]
		public async Task GetExchangeRatesAsync_DoesNotReturnRatesNotSpecifiedInCurrenciesParameter()
		{
			var currencies = new List<Currency>
			{
				new Currency("ABC"),
				new Currency("GHI")
			};

			mockDataStringParser
				.Setup(m => m.Parse(It.IsAny<string>()))
				.Returns(new[]
				{
					new ExchangeRate(new Currency("ABC"), new Currency("XYZ"), 1.23M),
					new ExchangeRate(new Currency("DEF"), new Currency("XYZ"), 3.45M),
					new ExchangeRate(new Currency("GHI"), new Currency("XYZ"), 6.78M)
				});

			var actualExchangeRates = await exchangeRateProvider.GetExchangeRatesAsync(currencies).ConfigureAwait(false);

			actualExchangeRates.ToList().Should()
				.BeEquivalentTo(new[]
				{
					new ExchangeRate(new Currency("ABC"), new Currency("XYZ"), 1.23M),
					new ExchangeRate(new Currency("GHI"), new Currency("XYZ"), 6.78M)
				}, options => options.WithStrictOrdering());
		}
	}
}