using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Services.Interfaces;
using ExchangeRateUpdater.Services.Models;
using ExchangeRateUpdater.Services.Models.External;
using ExchangeRateUpdater.Tests.Services.TestHelper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Tests.Services;

public class ExchangeRateProviderTests
{
    private readonly IApiClient<CnbRate> _apiClient;
    private readonly TestLogger<ExchangeRateProvider> _logger;
    private readonly ExchangeRateProvider _provider;

    public ExchangeRateProviderTests()
    {
        _apiClient = A.Fake<IApiClient<CnbRate>>();
        _logger = new TestLogger<ExchangeRateProvider>();
        _provider = new ExchangeRateProvider(_apiClient, _logger);
    }

    [Fact]
    public async Task GetExchangeRates_ShouldReturnRates_WhenCurrenciesAreValid()
    {
        // Arrange
        var currencies = new[] { new Currency("USD"), new Currency("EUR"), new Currency("CZK") };
        var apiRates = new List<CnbRate>
        {
            new CnbRate("USD", 23.5m, 1),
            new CnbRate("EUR", 25.0m, 10)
        };

        A.CallTo(() => _apiClient.GetExchangeRatesAsync())
            .Returns(Task.FromResult<IEnumerable<CnbRate>>(apiRates));

        // Act
        var result = await _provider.GetExchangeRates(currencies);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(r => r.SourceCurrency.Code == "USD" && r.TargetCurrency.Code == "CZK" && r.Value == 23.5m);
        result.Should().Contain(r => r.SourceCurrency.Code == "EUR" && r.TargetCurrency.Code == "CZK" && r.Value == 2.50m);
    }

    [Fact]
    public async Task GetExchangeRates_ShouldIgnoreTargetCurrency()
    {
        // Arrange
        var currencies = new[] { new Currency("CZK") };

        // Act
        var result = await _provider.GetExchangeRates(currencies);

        // Assert
        result.Should().BeEmpty();
        A.CallTo(() => _apiClient.GetExchangeRatesAsync())
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task GetExchangeRates_ShouldLogWarning_WhenSomeRatesAreMissing()
    {
        // Arrange
        var currencies = new[] { new Currency("USD"), new Currency("GBP") };
        var apiRates = new List<CnbRate>
        {
            new CnbRate("USD", 23.5m, 1)
        };

        A.CallTo(() => _apiClient.GetExchangeRatesAsync())
            .Returns(Task.FromResult<IEnumerable<CnbRate>>(apiRates));

        // Act
        var result = await _provider.GetExchangeRates(currencies);

        // Assert
        result.Should().HaveCount(1);
        result.Should().ContainSingle(r => r.SourceCurrency.Code == "USD");

        _logger.LogMessages.Should().Contain(m =>
            m.Message.Contains("GBP")
            && m.LogLevel == LogLevel.Warning);
    }

    [Fact]
    public async Task GetExchangeRates_ShouldThrow_WhenCurrenciesIsNull()
    {
        // Act
        var act = async () => await _provider.GetExchangeRates(null);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
        A.CallTo(() => _apiClient.GetExchangeRatesAsync())
            .MustNotHaveHappened();
    }
}
