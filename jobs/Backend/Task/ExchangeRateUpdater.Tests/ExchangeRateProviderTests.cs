using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateProviderTests
{
    #region GetExchangeRates

    [Fact]
    public async Task GetExchangeRates_NoDataProvided_ReturnsEmptyList()
    {
        // Arrange
        var urlResponse = String.Empty;
        
        var sut = this.GetSubjectUnderTest(urlResponse);
        var currencies = new[]
        {
            new Currency("USD"),
            new Currency("EUR")
        };

        // Act
        var result = await sut.GetExchangeRates(currencies);

        // Assert
        Assert.Empty(result);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\n")]
    [InlineData("EMU|1|EUR|24.820")]
    [InlineData("EMU|badExtraField|euro|1|EUR|24.820")]
    public async Task GetExchangeRates_InvalidDataset_HandlesCorrectly(string invalidData)
    {
        // Arrange
        var urlResponse = 
            $"28 Jan 2023 #19\nCountry|Currency|Amount|Code|Rate\n{invalidData}\nBrazil|real|1|BRL|4.287\n";
        
        var sut = this.GetSubjectUnderTest(urlResponse);
        var currencies = new[]
        {
            new Currency("BRL"),
            new Currency("EUR")
        };

        // Act
        var result = await sut.GetExchangeRates(currencies);

        // Assert
        Assert.Single(result);
        Assert.Contains(result, x => x.SourceCurrency.Code == "BRL");
    }
    
    [Fact]
    public async Task GetExchangeRates_MismatchedData_ReturnsEmptyList()
    {
        // Arrange
        var urlResponse = 
            "28 Jan 2023 #19\nCountry|Currency|Amount|Code|Rate\nBrazil|real|1|BRL|4.287\nBulgaria|lev|1|BGN|12.178";
        
        var sut = this.GetSubjectUnderTest(urlResponse);
        var currencies = new[]
        {
            new Currency("USD"),
            new Currency("EUR")
        };

        // Act
        var result = await sut.GetExchangeRates(currencies);

        // Assert
        Assert.Empty(result);
    }
    
    [Fact]
    public async Task GetExchangeRates_InvalidRate_HandlesCorrectly()
    {
        // Arrange
        var urlResponse = 
            "28 Jan 2023 #19\nCountry|Currency|Amount|Code|Rate\nBrazil|real|1|BRL|bad\nBulgaria|lev|1|BGN|12.178";
        
        var sut = this.GetSubjectUnderTest(urlResponse);
        var currencies = new[]
        {
            new Currency("BRL"),
            new Currency("BGN")
        };

        // Act
        var result = await sut.GetExchangeRates(currencies);

        // Assert
        Assert.Single(result);
        Assert.Contains(result, x => x.SourceCurrency.Code == "BGN");
    }
    
    [Fact]
    public async Task GetExchangeRates_PartialDataOverlap_ReturnsCorrectlyFilteredList()
    {
        // Arrange
        var urlResponse = 
            "28 Jan 2023 #19\nCountry|Currency|Amount|Code|Rate\nEMU|euro|1|EUR|24.820\nBulgaria|lev|1|BGN|12.178";
        
        var sut = this.GetSubjectUnderTest(urlResponse);
        var currencies = new[]
        {
            new Currency("USD"),
            new Currency("EUR")
        };

        // Act
        var result = await sut.GetExchangeRates(currencies);

        // Assert
        Assert.Single(result);

        Assert.Contains(result, x => x.SourceCurrency.Code == "EUR");
        Assert.DoesNotContain(result, x => x.SourceCurrency.Code == "USD");
    }
    
    [Fact]
    public async Task GetExchangeRates_2Records_FormatsBothCorrectly()
    {
        // Arrange
        var urlResponse = 
            "28 Jan 2023 #19\nCountry|Currency|Amount|Code|Rate\nEMU|euro|1|EUR|24.820\nUSA|dollar|1|USD|21.866";
        
        var sut = this.GetSubjectUnderTest(urlResponse);
        var currencies = new[]
        {
            new Currency("USD"),
            new Currency("EUR")
        };

        // Act
        var result = await sut.GetExchangeRates(currencies);

        // Assert
        Assert.Equal(2, result.Count());

        var eur = result.FirstOrDefault(x => x.SourceCurrency.Code == "EUR");
        Assert.NotNull(eur);
        Assert.Equal("CZK", eur?.TargetCurrency.Code);
        Assert.Equal(24.820m, eur?.Value);

        var usd = result.FirstOrDefault(x => x.SourceCurrency.Code == "USD");
        Assert.NotNull(usd);
        Assert.Equal("CZK", usd?.TargetCurrency.Code);
        Assert.Equal(21.866m, usd?.Value);
    }
    
    [Fact]
    public async Task GetExchangeRates_LargerDataset_RespondsCorrectly()
    {
        // Arrange
        var urlResponse = 
            "27 Jan 2023 #19\nCountry|Currency|Amount|Code|Rate\nAustralia|dollar|1|AUD|15.562\nBrazil|real|1|BRL|4.287\nBulgaria|lev|1|BGN|12.178\nCanada|dollar|1|CAD|16.353\nChina|renminbi|1|CNY|3.224\nDenmark|krone|1|DKK|3.202\nEMU|euro|1|EUR|23.820\nHongkong|dollar|1|HKD|2.793\nHungary|forint|100|HUF|6.149\nIceland|krona|100|ISK|15.220\nIMF|SDR|1|XDR|29.513\nIndia|rupee|100|INR|26.820\nIndonesia|rupiah|1000|IDR|1.463\nIsrael|new shekel|1|ILS|6.424\nJapan|yen|100|JPY|16.851\nMalaysia|ringgit|1|MYR|5.150\nMexico|peso|1|MXN|1.161\nNew Zealand|dollar|1|NZD|14.180\nNorway|krone|1|NOK|2.214\nPhilippines|peso|100|PHP|40.173\nPoland|zloty|1|PLN|5.047\nRomania|leu|1|RON|4.879\nSingapore|dollar|1|SGD|16.668\nSouth Africa|rand|1|ZAR|1.279\nSouth Korea|won|100|KRW|1.775\nSweden|krona|1|SEK|2.131\nSwitzerland|franc|1|CHF|23.812\nThailand|baht|100|THB|66.757\nTurkey|lira|1|TRY|1.162\nUnited Kingdom|pound|1|GBP|27.088\nUSA|dollar|1|USD|21.866\n";
        
        var sut = this.GetSubjectUnderTest(urlResponse);
        var currencies = new[]
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
        };

        // Act
        var result = await sut.GetExchangeRates(currencies);

        // Assert
        Assert.Equal(5, result.Count());
    }
    
    #endregion

    #region Setup

    private ExchangeRateProvider GetSubjectUnderTest(string response)
    {
        var httpMessageHandler = new Mock<HttpClientHandler>();
        
        httpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(response),
            });
        
        return new ExchangeRateProvider(new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://test.com/")
        });

    }
    
    #endregion
}