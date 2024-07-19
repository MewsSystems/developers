using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExchangeRateUpdater;
using ExchangeRateUpdater.ExternalVendors.CzechNationalBank;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace UnitTests;

public class ExchangeRateRetrieverTests
{
    private readonly Mock<ILogger<CzechNationalBankExchangeRateProvider>> _mockLogger = new();
    private readonly Mock<IExchangeRateClient> _mockClient = new();
    
    private readonly CzechNationalBankExchangeRateProvider _provider;

    public ExchangeRateRetrieverTests()
    {
        _provider = new CzechNationalBankExchangeRateProvider(_mockLogger.Object, _mockClient.Object);
    }

    [Theory]
    [ClassData(typeof(CurrencyTestData))]
    public async void ValidCurrencyRetrieval(List<Currency> currencies, int expectedReturn)
    {
        string file = await File.ReadAllTextAsync("./test_data/sample_api_response.json");
        ExchangeRateDto goodResult = JsonConvert.DeserializeObject<ExchangeRateDto>(file);
        
        _mockClient.Setup(c => c.GetDailyExchangeRates()).ReturnsAsync(goodResult);
        var results = await this._provider.GetExchangeRates(currencies);
        Assert.True(results.Count() == expectedReturn);
    }
}