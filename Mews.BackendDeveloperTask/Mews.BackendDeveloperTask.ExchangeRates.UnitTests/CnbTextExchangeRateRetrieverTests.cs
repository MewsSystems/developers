using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Mews.BackendDeveloperTask.ExchangeRates.Cnb;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace Mews.BackendDeveloperTask.ExchangeRates;

public class CnbTextExchangeRateRetrieverTests
{
    [Test]
    public async Task RetrievesExchangeRates()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.Is<HttpRequestMessage>(
                msg => msg.Method == HttpMethod.Get
                    && msg.RequestUri == new Uri("https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt")),
              ItExpr.IsAny<CancellationToken>())
           .ReturnsAsync(new HttpResponseMessage
           {
               StatusCode = HttpStatusCode.OK,
               Content = new StringContent("TEST CONTENT"),
           });

        var client = new HttpClient(mockHandler.Object);
        var retriever = new CnbTextExchangeRateRetriever(client);

        // Act
        var result = await retriever.GetDailyRatesAsync();

        // Assert
        Assert.AreEqual("TEST CONTENT", result);
    }
}