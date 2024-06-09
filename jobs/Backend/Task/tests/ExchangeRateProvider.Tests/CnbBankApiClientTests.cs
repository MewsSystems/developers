using ExchangeRateProvider.BankApiClients.Cnb;
using ExchangeRateProvider.Models;
using Moq;
using Moq.Protected;
using Shouldly;
using System.Net;

namespace ExchangeRateProvider.Tests;

public class CnbBankApiClientTests
{

    [Fact]
    public async Task Given_a_specific_api_response_Should_parse_it_correctly_and_return_common_model()
    {
        // Arrange
        const string responseContent =
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

        // Act
        var bankCurrencyRates = await bankApiClient.GetDailyExchangeRatesAsync();

        // Assert
        BankCurrencyRate[] expectedRates =
        [
            new BankCurrencyRate(1L, "AUD", 15.049m),
            new BankCurrencyRate(1L, "BRL", 4.281m),
            new BankCurrencyRate(100L, "JPY", 14.514m)
        ];

        bankCurrencyRates.ToArray().ShouldBeEquivalentTo(expectedRates);
    }
}
