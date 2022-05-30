using ExchangeRate.Client.Cnb;
using ExchangeRate.Client.Cnb.Abstract;
using ExchangeRate.Service.Service;
using Moq;
using Xunit;

namespace ExchangeRate.UnitTests.Service
{
	public class CnbXmlExchangeRateServiceTests
	{
		[Fact]
		public async Task GetExchangeRates_Xml()
		{
			var mockExchangeRateClient = new Mock<IExchangeRateClient>();
			mockExchangeRateClient.Setup(s => s.GetExchangeRatesXmlAsync()).ReturnsAsync(TestConstants.XmlExchangeRateExample);

			var exchangeRateService = new CnbXmlExchangeRateService(mockExchangeRateClient.Object);

			var result = await exchangeRateService.GetExchangeRates(CnbConstants.BaseCurrency);
			Assert.NotNull(result);
			Assert.True(result!.Count == 2);
			Assert.Equal($"EUR/CZK={24.45}", result[0]);
			Assert.Equal($"USD/CZK={22.515}", result[1]);
		}

		[Fact]
		public async Task GetExchangeRates_Xml_EmptyResultFromSource()
		{
			var mockExchangeRateClient = new Mock<IExchangeRateClient>();
			mockExchangeRateClient.Setup(s => s.GetExchangeRatesXmlAsync()).ReturnsAsync(TestConstants.XmlExchangeRateNotCompleteExample);
			var exchangeRateService = new CnbXmlExchangeRateService(mockExchangeRateClient.Object);

			var result = await exchangeRateService.GetExchangeRates(CnbConstants.BaseCurrency);
			Assert.Null(result);
		}
	}
}
