using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExchangeRateUpdater;
using ExchangeRateUpdater.ExternalVendors.CzechNationalBank;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace UnitTests.ExternalVendors.CzechNationalBank;

public class ExchangeRateProviderTests
{
    private readonly Mock<ILogger<CzechNationalBankExchangeRateProvider>> _mockLogger = new();
    private readonly Mock<IExchangeRateClient> _mockClient = new();
    private readonly IExchangeRateProvider _provider;

    public ExchangeRateProviderTests()
    {
        var cache = new MemoryCache(new MemoryCacheOptions());
        _provider = new CzechNationalBankExchangeRateProvider(_mockLogger.Object, _mockClient.Object, cache,
            Options.Create(new CzechNationalBankSettings()
            {
                RATE_STORAGE_KEY = "currentRates",
                REFRESH_RATE_IN_MINUTES = 15
            }));
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