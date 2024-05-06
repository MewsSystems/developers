using System.Net;
using System.Text.Json;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Exceptions;
using ExchangeRateUpdater.Infrastructure.Clients;
using ExchangeRateUpdater.Infrastructure.Dto;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Moq.Protected;

namespace ExchangeRateUpdater.Infrastructure.Tests.Clients;

public class CzechNationalBankExchangeRateClientTest
{
	private const string BaseAddress = "https://test.com/";

	[Fact]
	public async void FetchExchangeRates_FormsCorrectUrlWithParams()
	{
		// Arrange
		const string language = "testlang";
		var date = new DateOnly(2024, 1, 21);
		var (exchangeRateClient, messageHandler) = GetMockExchangeRateClient(new HttpResponseMessage
		{
			StatusCode = HttpStatusCode.OK,
			Content = new StringContent("{}")
		});

		// Act
		_ = await exchangeRateClient.FetchExchangeRates(language, date);

		// Assert
		messageHandler.Protected().Verify("SendAsync", Times.Once(),
			ItExpr.Is<HttpRequestMessage>(req =>
				req.RequestUri!.ToString() == $"{BaseAddress}daily?date=2024-01-21&lang={language}"),
			ItExpr.IsAny<CancellationToken>());
	}

	[Fact]
	public async void FetchExchangeRates_ReturnsExchangeRatesIfSuccessfulExternalCall()
	{
		// Arrange
		var response = new ExRateDailyResponse([
			new ExRateDailyRest(1L, "Australia", "dollar", "AUD", 95, 16.487m, "2022-05-17"),
			new ExRateDailyRest(1L, "Brazil", "real", "BRL", 95, 4.697m, "2022-05-17"),
			new ExRateDailyRest(1000L, "Indonesia", "rupiah", "IDR", 95, 1.601m, "2022-05-17")
		]);
		var (exchangeRateClient, _) = GetMockExchangeRateClient(new HttpResponseMessage
		{
			StatusCode = HttpStatusCode.OK,
			Content = new StringContent(JsonSerializer.Serialize(response))
		});

		// Act
		var exchangeRates = await exchangeRateClient.FetchExchangeRates("en", new DateOnly(2024, 1, 1));

		// Assert
		var targetCurrency = new Currency("CZK");
		exchangeRates.Should().BeEquivalentTo(new List<ExchangeRate>
		{
			new(new Currency("AUD"), targetCurrency, 16.487m),
			new(new Currency("BRL"), targetCurrency, 4.697m),
			new(new Currency("IDR"), targetCurrency, 0.001601m),
		});
	}

	[Theory]
	[InlineData(HttpStatusCode.NotFound)]
	[InlineData(HttpStatusCode.BadRequest)]
	public async void FetchExchangeRates_ThrowsExceptionIfErrorStatusCodeReceived(HttpStatusCode statusCode)
	{
		// Arrange
		var (exchangeRateClient, _) = GetMockExchangeRateClient(new HttpResponseMessage { StatusCode = statusCode });

		// Act
		var act = () => exchangeRateClient.FetchExchangeRates("an", new DateOnly(2024, 1, 1));

		// Assert
		await act.Should().ThrowAsync<FetchExchangeRatesFailException>()
				.WithMessage("Failed to fetch exchange rates*");
	}

	[Fact]
	public async void FetchExchangeRates_ThrowsExceptionIfUnexpectedResponse()
	{
		// Arrange
		var (exchangeRateClient, _) = GetMockExchangeRateClient(new HttpResponseMessage
			{ StatusCode = HttpStatusCode.OK, Content = new StringContent("") });

		// Act
		var act = () => exchangeRateClient.FetchExchangeRates("an", new DateOnly(2024, 1, 1));

		// Assert
		await act.Should().ThrowAsync<FetchExchangeRatesFailException>()
				.WithMessage("Unexpected error occurred when fetching exchange rates.");
	}

	private static (CzechNationalBankExchangeRateClient, Mock<HttpMessageHandler>) GetMockExchangeRateClient(
		HttpResponseMessage responseMessage)
	{
		var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
		mockHttpMessageHandler.Protected()
							.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
								ItExpr.IsAny<CancellationToken>())
							.ReturnsAsync(responseMessage);
		var httpClient = new HttpClient(mockHttpMessageHandler.Object)
		{
			BaseAddress = new Uri(BaseAddress),
		};

		ILogger<CzechNationalBankExchangeRateClient> logger = new NullLogger<CzechNationalBankExchangeRateClient>();
		var exchangeClient = new CzechNationalBankExchangeRateClient(httpClient, logger);
		return (exchangeClient, mockHttpMessageHandler);
	}
}