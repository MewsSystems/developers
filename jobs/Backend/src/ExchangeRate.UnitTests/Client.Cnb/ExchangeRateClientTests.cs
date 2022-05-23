using ExchangeRate.Client.Cnb;
using ExchangeRate.Client.Cnb.Models;
using ExchangeRate.UnitTests.Common;
using Framework.Converters;
using Framework.Converters.Abstract;
using Framework.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Xunit;

namespace ExchangeRate.UnitTests.Client.Cnb
{
	public class ExchangeRateClientTests
	{
		[Fact]
		public async Task GetExchangeRatesXmlAsync_GetOkResponse()
		{
			var mockHttpClient = CreateHttpClientForMoq(TestConstants.HttpResponseMessageXmlExample);
			var mockCnbConfiguration = new Mock<IOptions<CnbClientConfiguration>>();
			var mockLogger = new Mock<ILogger<CnbExchangeRateClient>>();
			var mockXmlConverter = new Mock<ILogger<XmlConverter>>();

			MoqData.SetupMockCnbConfiguration(mockCnbConfiguration);

			var client = new CnbExchangeRateClient(mockHttpClient, mockCnbConfiguration.Object, mockLogger.Object, new XmlConverter(mockXmlConverter.Object));

			var result = await client.GetExchangeRatesXmlAsync();
			Assert.NotNull(result);
			Assert.NotNull(result.Table);
			Assert.NotNull(result.Table?.Rows);
			Assert.True(result.Table?.Rows?.Count == 2);

			var item = result.Table?.Rows?[0];
			Assert.NotNull(item);
			Assert.Equal("EUR", item?.Code);
			Assert.Equal("euro", item?.CurrencyName);
			Assert.Equal(1, item?.Amount);
			Assert.True((decimal)24.450 == item?.Rate);
			Assert.Equal("EMU", item?.Country);

			item = result.Table?.Rows?[1];
			Assert.NotNull(item);
			Assert.Equal("USD", item?.Code);
			Assert.Equal("dolar", item?.CurrencyName);
			Assert.Equal(1, item?.Amount);
			Assert.True((decimal)22.515 == item?.Rate);
			Assert.Equal("USA", item?.Country);
		}

		[Fact]
		public async Task GetExchangeRatesTxtAsync_GetOkResponse()
		{
			var mockHttpClient = CreateHttpClientForMoq(TestConstants.HttpResponseMessageTxtExample);
			var mockCnbConfiguration = new Mock<IOptions<CnbClientConfiguration>>();
			var mockLogger = new Mock<ILogger<CnbExchangeRateClient>>();
			var mockXmlConverter = new Mock<IXmlConverter>();

			MoqData.SetupMockCnbConfiguration(mockCnbConfiguration);

			var client = new CnbExchangeRateClient(mockHttpClient, mockCnbConfiguration.Object, mockLogger.Object, mockXmlConverter.Object);

			var result = await client.GetExchangeRatesTxtAsync();
			Assert.NotNull(result);
			Assert.True(result.Count == 2);

			var item = result[0];
			Assert.NotNull(item);
			Assert.Equal("EUR", item.Code);
			Assert.Equal("euro", item.Currency);
			Assert.Equal(1, item.Amount);
			Assert.True((decimal)24.450 == item.Rate);
			Assert.Equal("EMU", item.Country);

			item = result[1];
			Assert.NotNull(item);
			Assert.Equal("USD", item.Code);
			Assert.Equal("dolar", item.Currency);
			Assert.Equal(1, item.Amount);
			Assert.True((decimal)22.515 == item.Rate);
			Assert.Equal("USA", item.Country);
		}

		[Fact]
		public async Task GetExchangeRatesTxtAsync_ParsingException()
		{
			var mockHttpClient = CreateHttpClientForMoq(TestConstants.HttpResponseMessageParsingErrorTxtExample);
			var mockCnbConfiguration = new Mock<IOptions<CnbClientConfiguration>>();
			var mockLogger = new Mock<ILogger<CnbExchangeRateClient>>();
			var mockXmlConverter = new Mock<IXmlConverter>();

			MoqData.SetupMockCnbConfiguration(mockCnbConfiguration);

			var client = new CnbExchangeRateClient(mockHttpClient, mockCnbConfiguration.Object, mockLogger.Object, mockXmlConverter.Object);

			var exception = await Assert.ThrowsAsync<ParsingException>(async () => await client.GetExchangeRatesTxtAsync());
			Assert.Equal("Content from CNB txt api request has invalid format", exception.Message);
		}

		[Fact]
		public async Task GetExchangeRatesTxtAsync_ArgumentNullException()
		{
			var mockHttpClient = CreateHttpClientForMoq(TestConstants.HttpResponseMessageEmptyResponseContentTxtExample);
			var mockCnbConfiguration = new Mock<IOptions<CnbClientConfiguration>>();
			var mockLogger = new Mock<ILogger<CnbExchangeRateClient>>();
			var mockXmlConverter = new Mock<IXmlConverter>();

			MoqData.SetupMockCnbConfiguration(mockCnbConfiguration, "");

			var client = new CnbExchangeRateClient(mockHttpClient, mockCnbConfiguration.Object, mockLogger.Object, mockXmlConverter.Object);

			var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => await client.GetExchangeRatesTxtAsync());
			Assert.Equal("Parameter: URL cannot be null or empty.", exception.Message);
		}

		[Fact]
		public async Task GetExchangeRatesTxtAsync_EmptyResponseContent()
		{
			var mockHttpClient = CreateHttpClientForMoq(TestConstants.HttpResponseMessageEmptyResponseContentTxtExample);
			var mockCnbConfiguration = new Mock<IOptions<CnbClientConfiguration>>();
			var mockLogger = new Mock<ILogger<CnbExchangeRateClient>>();
			var mockXmlConverter = new Mock<IXmlConverter>();

			MoqData.SetupMockCnbConfiguration(mockCnbConfiguration);

			var client = new CnbExchangeRateClient(mockHttpClient, mockCnbConfiguration.Object, mockLogger.Object, mockXmlConverter.Object);

			var exception = await Assert.ThrowsAsync<EmptyResultSetException>(async () => await client.GetExchangeRatesTxtAsync());
			Assert.Equal("No content available for CNB exchange rate request", exception.Message);
		}

		[Fact]
		public async Task GetExchangeRatesTxtAsync_NoContentResponse()
		{
			var mockHttpClient = CreateHttpClientForMoq(new HttpResponseMessage());
			var mockCnbConfiguration = new Mock<IOptions<CnbClientConfiguration>>();
			var mockLogger = new Mock<ILogger<CnbExchangeRateClient>>();
			var mockXmlConverter = new Mock<IXmlConverter>();

			MoqData.SetupMockCnbConfiguration(mockCnbConfiguration);

			var client = new CnbExchangeRateClient(mockHttpClient, mockCnbConfiguration.Object, mockLogger.Object, mockXmlConverter.Object);

			var exception = await Assert.ThrowsAsync<EmptyResultSetException>(async () => await client.GetExchangeRatesTxtAsync());
			Assert.Equal("No content available for CNB exchange rate request", exception.Message);
		}

		[Fact]
		public void GetExchangeRatesTxtAsync_CheckClientConfiguration()
		{
			var mockCnbConfiguration = new Mock<IOptions<CnbClientConfiguration>>();

			MoqData.SetupMockCnbConfiguration(mockCnbConfiguration);

			Assert.NotNull(mockCnbConfiguration.Object.Value);
			Assert.True(mockCnbConfiguration.Object.Value.CacheTtl == 5);
			Assert.True(mockCnbConfiguration.Object.Value.Retry == 3);
			Assert.True(mockCnbConfiguration.Object.Value.CnbTxtClient?.Url == "https://www.cnb.cz/");
			Assert.True(mockCnbConfiguration.Object.Value.CnbXmlClient?.Url == "https://www.cnb.cz/");
		}

		#region private members

		private static HttpClient CreateHttpClientForMoq(HttpResponseMessage response)
		{
			var handlerMock = new Mock<HttpMessageHandler>();
			handlerMock
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(response);
			var httpClient = new HttpClient(handlerMock.Object);
			return httpClient;
		}

		#endregion
	}
}
