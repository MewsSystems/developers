using System.Net;
using ExchangeRateProvider.BankApiClients.Cnb;
using ExchangeRateProvider.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Shouldly;

namespace ExchangeRateProvider.Tests;

public class ExchangeRateProviderIntegrationTests
{

	[Fact]
	public async Task When_exchange_provider_is_called_with_currencies_not_returned_by_api_Then_those_are_ignored()
	{
		// Arrange
		var responseContent =
			"""
			{
			  "rates": [
			    {
			      "validFor": "2024-06-06",
			      "order": 109,
			      "country": "Australia",
			      "currency": "dollar",
			      "amount": 1,
			      "currencyCode": "AUD",
			      "rate": 15.049
			    },
			    {
			      "validFor": "2024-06-06",
			      "order": 109,
			      "country": "Brazil",
			      "currency": "real",
			      "amount": 1,
			      "currencyCode": "BRL",
			      "rate": 4.281
			    },
			    {
			      "validFor": "2024-06-06",
			      "order": 109,
			      "country": "Japan",
			      "currency": "yen",
			      "amount": 100,
			      "currencyCode": "JPY",
			      "rate": 14.514
			    }
			  ]
			}
			""";

		var httpMessageHandlerMock = new Mock<HttpMessageHandler>();

		httpMessageHandlerMock
			.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>())
			.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new StringContent(responseContent)
			});

		var httpClient = new HttpClient(httpMessageHandlerMock.Object)
		{
			BaseAddress = new Uri("https://test.local")
		};
		var bankApiClient = new CnbBankApiClient(httpClient);

		var cache = new MemoryCache(new MemoryCacheOptions());
		var logger = new Mock<ILogger<IExchangeRateProvider>>();

		// Act
		var exchangeRatesProvider = new ExchangeRateProvider(bankApiClient, cache, logger.Object, TimeProvider.System);


		var currencies = new[] { "USD", "EUR", "JPY" }.Select(c => new Currency(c));
		var rates = await exchangeRatesProvider.GetExchangeRatesAsync(currencies);

		// Assert
		rates.ShouldHaveSingleItem();
		rates.Single().SourceCurrency.Code.ShouldBe("JPY");

	}

}

