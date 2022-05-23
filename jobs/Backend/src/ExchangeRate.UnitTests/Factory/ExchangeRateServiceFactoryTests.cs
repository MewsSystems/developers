using ExchangeRate.Client.Cnb;
using ExchangeRate.Client.Cnb.Abstract;
using ExchangeRate.Service.Factory;
using ExchangeRate.Service.Service;
using Moq;
using Xunit;

namespace ExchangeRate.UnitTests.Factory
{
	public class ExchangeRateServiceFactoryTests
	{
		[Fact]
		public void GetExchangeRateService_Xml()
		{
			var mockExchangeRateClient = new Mock<IExchangeRateClient>();

			var exchangeRateService = new ExchangeRateServiceFactory(mockExchangeRateClient.Object);

			var result = exchangeRateService.GetExchangeRateService(CnbConstants.ApiType.CnbXml);
			Assert.NotNull(result);
			Assert.True(result.GetType() == typeof(CnbXmlExchangeRateService));
		}

		[Fact]
		public void GetExchangeRateService_Txt()
		{
			var mockExchangeRateClient = new Mock<IExchangeRateClient>();

			var exchangeRateService = new ExchangeRateServiceFactory(mockExchangeRateClient.Object);

			var result = exchangeRateService.GetExchangeRateService(CnbConstants.ApiType.CnbTxt);
			Assert.NotNull(result);
			Assert.True(result.GetType() == typeof(CnbTxtExchangeRateService));
		}
	}
}
