using System.Net;
using Moq;
using Moq.Protected;
using ExchangeRateUpdater.src;
using ExchangeRateUpdaterModels.Models;

public class CzechNationalBankApiTests
{
    [Fact]
    public async Task GetRatesAsync_ReturnsCorrectRates()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<System.Threading.CancellationToken>()
           )
           .ReturnsAsync(new HttpResponseMessage
           {
               StatusCode = HttpStatusCode.OK,
               Content = new StringContent(@" 
                { 'rates': 
                    [ 
                        {'validFor': '2024-11-08', 'order': 218, 'country': 'Austrálie', 'currency': 'dolar', 'amount': 1, 'currencyCode': 'AUD', 'rate': 15.501 },
                        { 'validFor': '2024-11-08', 'order': 218, 'country': 'Brazílie', 'currency': 'real', 'amount': 1, 'currencyCode': 'BRL', 'rate': 4.062 }
                    ] 
                }")
           })
           .Verifiable();

        var httpClient = new HttpClient(handlerMock.Object);
        var api = new CzechNationalBankAPI(httpClient);

        // Act
        IEnumerable<ExchangeRateModel> rates = await api.GetRatesAsync();

        // Assert
        Assert.Equal(2, rates.Count());
        Assert.Contains(rates, r => r.SourceCurrency.Code == "AUD" && r.Value == 15.501m);
        Assert.Contains(rates, r => r.SourceCurrency.Code == "BRL" && r.Value == 4.062m);
    }
}
