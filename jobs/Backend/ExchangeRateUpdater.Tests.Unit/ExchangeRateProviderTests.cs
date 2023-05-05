using System.Net;
using ExchangeRateUpdater;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace Exception.Tests.Unit;

[TestFixture]
public class ExchangeRateProviderTests
{
    private HttpClient ?_httpClient;
    private ExchangeRateProvider ?_exchangeRateProvider;
    

    [SetUp]
    public void SetUp()
    {
      
        
    }

    //considerred using TestCase/TestCaseSource here to pass in working variants of sample data but decided against it.
    [Test]
    public async Task GetExchangeRatesAsync_ValidData_ReturnsParsedRates()
    {
        //Arange

        var _sampleData = @"Date, 5.5.2023
                        Country|Currency|Amount|Code|Rate
                        Australia|dollar|1|AUD|12.34
                        Japan|yen|100|JPY|15.67
                                               ";

        _httpClient = SetupHttpClientResponse(HttpStatusCode.OK, _sampleData);
        _exchangeRateProvider = new ExchangeRateProvider(_httpClient);

        var currencies = new List<Currency> { new Currency("AUD"), new Currency("JPY") };
       
        // Act
        var exchangeRates = await _exchangeRateProvider.GetExchangeRatesAsync(currencies);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(exchangeRates.Count(), Is.EqualTo(2));
            Assert.That(exchangeRates.First().SourceCurrency.Code, Is.EqualTo("AUD"));
            Assert.That(exchangeRates.First().Value, Is.EqualTo(12.34m));
            Assert.That(exchangeRates.Last().SourceCurrency.Code, Is.EqualTo("JPY"));
            Assert.That(exchangeRates.Last().Value, Is.EqualTo(15.67m));
        });
    }

    [Test]
    public void GetExchangeRatesAsync_InvalidDataFormat_ThrowsException()
    {
        // Arrange
        var currencies = new List<Currency> { new Currency("AUD"), new Currency("JPY") };

        //Missing rate for currency.
        var _sampleData = @"Date, 5.5.2023
                        Country|Currency|Amount|Code|Rate
                        Australia|dollar|1|AUD
                                                ";
        _httpClient = SetupHttpClientResponse(HttpStatusCode.OK, _sampleData);
        _exchangeRateProvider = new ExchangeRateProvider(_httpClient);

        // Act and Assert
        var exception = Assert.ThrowsAsync<System.Exception>(async () => await _exchangeRateProvider.GetExchangeRatesAsync(currencies));
        Assert.That(exception.Message, Is.EqualTo("Invalid format on line: Australia|dollar|1|AUD"));
    }

    [Test]
    public async Task GetExchangeRatesAsync_MissingRequiredFields_ReturnsOnlyValidRates()
    {
        // Arrange
        var currencies = new List<Currency> { new Currency("AUD"), new Currency("JPY") };
        var sampleData = @"Date, 5.5.2023
                        Country|Currency|Amount|Code|Rate
                        Australia|dollar|1|AUD|12.34
                        Japan|Yen|100||15.67
                                            ";
        _httpClient = SetupHttpClientResponse(HttpStatusCode.OK, sampleData);
        _exchangeRateProvider = new ExchangeRateProvider(_httpClient);

        // Act
        var exchangeRates = await _exchangeRateProvider.GetExchangeRatesAsync(currencies);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(exchangeRates.Count(), Is.EqualTo(1));
            Assert.That(exchangeRates.First().SourceCurrency.Code, Is.EqualTo("AUD"));
            Assert.That(exchangeRates.First().Value, Is.EqualTo(12.34m));
        });
    }

    [Test]
    public void GetExchangeRatesAsync_UnsuccessfulResponse_ThrowsException()
    {
        // Arrange
        var currencies = new List<Currency> { new Currency("AUD"), new Currency("JPY") };
        var sampleData = "Not found";
        _httpClient = SetupHttpClientResponse(HttpStatusCode.NotFound, sampleData);
        _exchangeRateProvider = new ExchangeRateProvider(_httpClient);

        // Act & Assert
        var exception = Assert.ThrowsAsync<System.Exception>(
            async () => await _exchangeRateProvider.GetExchangeRatesAsync(currencies));
        Assert.That(exception.Message, Is.EqualTo("Failed to retrieve exchange rates: NotFound"));
    }

    [Test]
    public async Task GetExchangeRatesAsync_EmptyCurrencies_ReturnsEmptyList()
    {
        // Arrange
        var sampleData = @"Date, 5.5.2023
                        Country|Currency|Amount|Code|Rate
                        Australia|dollar|1|AUD|12.34
                        Japan|yen|100|JPY|15.67
                        ";
        _httpClient = SetupHttpClientResponse(HttpStatusCode.OK, sampleData);
        _exchangeRateProvider = new ExchangeRateProvider(_httpClient);

        var currencies = new List<Currency>();

        // Act
        var exchangeRates = await _exchangeRateProvider.GetExchangeRatesAsync(currencies);

        // Assert
        Assert.IsEmpty(exchangeRates);
    }

    [Test]
    public void GetExchangeRatesAsync_InvalidStartingIndex_ThrowsException()
    {
        // Arrange
        var currencies = new List<Currency> { new Currency("AUD"), new Currency("JPY") };

        var sampleData = @"Date, 5.5.2023
                        Invalid|Header|Line|Here
                        Australia|dollar|1|AUD|12.34
                        Japan|yen|100|JPY|15.67
                        ";
        _httpClient = SetupHttpClientResponse(HttpStatusCode.OK, sampleData);
        _exchangeRateProvider = new ExchangeRateProvider(_httpClient);

        // Act and Assert
        var exception = Assert.ThrowsAsync<System.Exception>(async () => await _exchangeRateProvider.GetExchangeRatesAsync(currencies));
        Assert.That(exception.Message, Is.EqualTo("Starting index not found in exchange rate data"));
    }


    private HttpClient SetupHttpClientResponse(HttpStatusCode statusCode, string content)
    {
        var httpResponseMessage = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(content)
        };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponseMessage);

        return new HttpClient(handlerMock.Object);
    }
}
