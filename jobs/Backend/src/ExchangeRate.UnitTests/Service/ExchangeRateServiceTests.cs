using ExchangeRate.Client.Cnb;
using ExchangeRate.Client.Cnb.Abstract;
using ExchangeRate.Client.Cnb.Models;
using ExchangeRate.Client.Cnb.Models.Xml;
using ExchangeRate.Domain;
using ExchangeRate.Service.Abstract;
using ExchangeRate.Service.Service;
using ExchangeRate.UnitTests.Common;
using Framework.Caching.Abstract;
using Framework.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace ExchangeRate.UnitTests.Service
{
	public class ExchangeRateServiceTests
	{
		[Theory]
		[InlineData(CnbConstants.ApiType.CnbXml, "Cache item 1")]
		[InlineData(CnbConstants.ApiType.CnbTxt, "Cache item 2")]
		public async Task GetExchangeRates_ReturnFromCache(CnbConstants.ApiType apiType, string expectedCacheValue)
		{
			var mockExchangeRateServiceFactory = new Mock<IExchangeRateServiceFactory>();
			var mockCache = new Mock<ICache>();
			var logger = new Mock<ILogger<ExchangeRateService>>();
			var mockCnbConfiguration = new Mock<IOptions<CnbClientConfiguration>>();

			mockCache.Setup(s => s.Get<List<string>?>(It.IsAny<string>())).Returns(new List<string>() { expectedCacheValue });

			var exchangeRateService = new ExchangeRateService(mockExchangeRateServiceFactory.Object, mockCache.Object, logger.Object, mockCnbConfiguration.Object);

			var result = await exchangeRateService.GetExchangeRates(apiType);

			Assert.NotNull(result);
			var item = result?.FirstOrDefault();
			Assert.Equal(expectedCacheValue, item);
		}

		[Theory]
		[InlineData(CnbConstants.ApiType.CnbXml, "EUR", 24.450, 1, "EUR/CZK=24,45")]
		[InlineData(CnbConstants.ApiType.CnbXml, "USD", 22.515, 1, "USD/CZK=22,515")]
		public async Task GetExchangeRates_ReturnFromXmlClient(CnbConstants.ApiType apiType, string sourceCurrencyCode, decimal rate, int amount, string expected)
		{
			// Arrange
			var data = CreateXmlExchangeRateTable(sourceCurrencyCode, rate, amount);
			var exchangeRate = new ExchangeRate.Domain.ExchangeRate(new Currency(sourceCurrencyCode), new Currency(CnbConstants.BaseCurrency), rate);
			var exchangeRateService = PrepareExchangeRateService(data, apiType, exchangeRate);

			// Act
			var result = await exchangeRateService.GetExchangeRates(apiType);

			// Assert
			Assert.NotNull(result);
			var item = result!.FirstOrDefault();
			Assert.Equal(expected, item);
		}

		[Fact]
		public async Task GetExchangeRates_Txt_EmptyResult()
		{
			// Arrange
			var apiType = CnbConstants.ApiType.CnbXml;
			var data = CreateXmlExchangeRateTable("ABC", 1, 1);
			var exchangeRate = new ExchangeRate.Domain.ExchangeRate(new Currency("ABC"), new Currency(CnbConstants.BaseCurrency), 1);
			var exchangeRateService = PrepareExchangeRateService(data, apiType, exchangeRate);

			// Act
			var exception = await Assert.ThrowsAsync<EmptyResultSetException>(async () => await exchangeRateService.GetExchangeRates(CnbConstants.ApiType.CnbXml));

			// Assert
			Assert.Equal("Empty exchange rate data", exception.Message);
		}

		#region private members

		private static XmlExchangeRate CreateXmlExchangeRateTable(string sourceCurrencyCode, decimal rate, int amount)
		{
			return new XmlExchangeRate
			{
				Table = new XmlExchangeRateTable
				{
					Rows = new List<XmlExchangeRateRow>
					{
						XmlExchangeRateRow(sourceCurrencyCode, rate, amount)
					},
					Type = "XML_TYP_CNB_KURZY_DEVIZOVEHO_TRHU"
				},
				Bank = "CNB",
				Date = Convert.ToDateTime("20.04.2022"),
				OrderNo = 80
			};
		}

		private static XmlExchangeRateRow XmlExchangeRateRow(string sourceCurrencyCode, decimal rate, int amount)
		{
			return new XmlExchangeRateRow
			{
				Code = sourceCurrencyCode,
				Rate = rate,
				Amount = amount
			};
		}

		private static ExchangeRateService PrepareExchangeRateService(XmlExchangeRate data, CnbConstants.ApiType apiType, ExchangeRate.Domain.ExchangeRate exchangeRate)
		{
			var mockExchangeRateServiceFactory = new Mock<IExchangeRateServiceFactory>();
			var mockCache = new Mock<ICache>();
			var mockLogger = new Mock<ILogger<ExchangeRateService>>();
			var mockCnbConfiguration = new Mock<IOptions<CnbClientConfiguration>>();
			var mockCnbExchangeRateClient = new Mock<IExchangeRateClient>();

			MoqData.SetupMockCnbConfiguration(mockCnbConfiguration);
			mockCache.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<List<string>?>(), 0));

			mockCnbExchangeRateClient.Setup(s => s.GetExchangeRatesXmlAsync()).ReturnsAsync(data);
			var cnbXmlExchangeRateService = new CnbXmlExchangeRateService(mockCnbExchangeRateClient.Object);
			mockExchangeRateServiceFactory.Setup(s => s.GetExchangeRateService(apiType)).Returns(cnbXmlExchangeRateService);

			var mockCnbXmlExchangeRateService = new Mock<IConcreteExchangeRateService>();
			mockCnbXmlExchangeRateService.Setup(s => s.GetExchangeRates(It.IsAny<string>())).ReturnsAsync(new List<string>() { exchangeRate.ToString() });

			var exchangeRateService = new ExchangeRateService(mockExchangeRateServiceFactory.Object, mockCache.Object, mockLogger.Object, mockCnbConfiguration.Object);
			return exchangeRateService;
		}

		#endregion
	}
}
