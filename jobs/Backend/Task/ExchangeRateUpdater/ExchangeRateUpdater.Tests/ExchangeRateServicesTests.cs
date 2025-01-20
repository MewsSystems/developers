
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Tests;

[TestFixture]
public class ExchangeRateServicesTests
{
    private Mock<IExchangeRateRepository> _mockRepository;
    private Mock<ILogger<ExchangeRateService>> _mockLogger;
    private ExchangeRateService _service;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new Mock<IExchangeRateRepository>();
        _mockLogger = new Mock <ILogger<ExchangeRateService>>();
        _service = new ExchangeRateService(_mockRepository.Object, _mockLogger.Object);
    }


    [Test]    
    public async Task GivenInalidExchangeRate_ShouldReturnEmptyList()
    {
        // Arrange            
        var currencies = new List<ExchangeRateRequest>()
            {
                new ExchangeRateRequest()
                {
                    SourceCurrency = new Currency("CZK"),
                    TargetCurrency = new Currency("ABC")
                }
            };
        
        var resultado = new List<ExchangeRate>();
        _mockRepository.Setup(s => s.GetExchangeRatesByDateAsync(new DateTime(2025, 01, 01), new CancellationToken())).ReturnsAsync(resultado);

        //// Act
        var result = await _service.GetExchangeRates(currencies, new DateTime(2025, 01, 01), new CancellationToken());

        // Assert
        Assert.IsNotNull(result);                
        Assert.IsEmpty(result);
        Assert.IsFalse(result.Any());
    }

    [Test]
    public async Task GivenValidExchangeRate_ShouldReturnExchangeRate()
    {
        // Arrange            
        var currencies = new List<ExchangeRateRequest>()
            {
                new ExchangeRateRequest()
                {
                    SourceCurrency = new Currency("CZK"),
                    TargetCurrency = new Currency("USD")
                }
            };

        var resultado = new List<ExchangeRate>()
        {
            new ExchangeRate(sourceCurrency: new Currency("CZK"), 
            targetCurrency: new Currency("USD"), 
            value: 24,  
            date: new DateTime(2025, 01, 20))
        };

        _mockRepository.Setup(s => s.GetExchangeRatesByDateAsync(new DateTime(2025, 01, 01), new CancellationToken())).ReturnsAsync(resultado);

        //// Act
        var result = await _service.GetExchangeRates(currencies, new DateTime(2025, 01, 01), new CancellationToken());

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotEmpty(result);
        Assert.IsTrue(result.Any());
        Assert.AreEqual("CZK / USD", result[0].Currencies);
    }
}
