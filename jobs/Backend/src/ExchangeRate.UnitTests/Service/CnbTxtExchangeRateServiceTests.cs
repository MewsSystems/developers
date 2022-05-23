using ExchangeRate.Client.Cnb;
using ExchangeRate.Client.Cnb.Abstract;
using ExchangeRate.Client.Cnb.Models;
using ExchangeRate.Client.Cnb.Models.Txt;
using ExchangeRate.Service.Service;
using ExchangeRate.UnitTests.Common;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace ExchangeRate.UnitTests.Service
{
	public class CnbTxtExchangeRateServiceTests
	{
		[Fact]
		public async Task GetExchangeRates_Txt()
		{
			var mockExchangeRateClient = new Mock<IExchangeRateClient>();
			var mockCnbConfiguration = new Mock<IOptions<CnbClientConfiguration>>();

			MoqData.SetupMockCnbConfiguration(mockCnbConfiguration);
			mockExchangeRateClient.Setup(s => s.GetExchangeRatesTxtAsync()).ReturnsAsync(TestConstants.TxtExchangeRateExample);

			var exchangeRateService = new CnbTxtExchangeRateService(mockExchangeRateClient.Object);

			var result = await exchangeRateService.GetExchangeRates(CnbConstants.BaseCurrency);

			Assert.NotNull(result);
			Assert.True(result!.Count == 2);
			Assert.Equal("EUR/CZK=24,45", result[0]);
			Assert.Equal("USD/CZK=22,515", result[1]);
		}

		[Fact]
		public async Task GetExchangeRates_Txt_EmptyResultFromSource()
		{
			var mockExchangeRateClient = new Mock<IExchangeRateClient>();
			mockExchangeRateClient.Setup(s => s.GetExchangeRatesTxtAsync()).ReturnsAsync(new List<TxtExchangeRate>());

			var exchangeRateService = new CnbTxtExchangeRateService(mockExchangeRateClient.Object);
			var result = await exchangeRateService.GetExchangeRates(CnbConstants.BaseCurrency);

			Assert.NotNull(result);
			Assert.True(result!.Count == 0);
		}
	}
}
