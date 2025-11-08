using ExchangeRateProviders.Core;
using ExchangeRateProviders.Core.Model;
using ExchangeRateProviders.Tests.TestHelpers;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace ExchangeRateProviders.Tests;

[TestFixture]
public class ExchangeRateServiceTests
{
    private IExchangeRateDataProviderFactory _dataProviderFactory = null!;
    private IExchangeRateDataProvider _dataProvider = null!;
    private ILogger<ExchangeRateService> _logger = null!;
    private ExchangeRateService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _dataProviderFactory = Substitute.For<IExchangeRateDataProviderFactory>();
        _dataProvider = Substitute.For<IExchangeRateDataProvider>();
        _logger = Substitute.For<ILogger<ExchangeRateService>>();
        _service = new ExchangeRateService(_dataProviderFactory, _logger);
    }

    [Test]
    public async Task GetExchangeRatesAsync_NullCurrencies_ReturnsEmpty()
    {
        // Act
        var result = await _service.GetExchangeRatesAsync("CZK", null!, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Empty);
            _logger.VerifyLogWarning(1, "Requested currencies collection is null. Returning empty result.");
        });
    }

    [Test]
    public async Task GetExchangeRatesAsync_FiltersToRequestedCurrencies()
    {
        // Arrange
        var targetCurrency = "CZK";
        var allRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 22.5m),
            new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 24.0m),
            new ExchangeRate(new Currency("JPY"), new Currency("CZK"), 0.17m)
        };
        
        _dataProvider.ExchangeRateProviderTargetCurrencyCode.Returns(targetCurrency);
        _dataProvider.GetDailyRatesAsync(Arg.Any<CancellationToken>()).Returns(allRates);
        _dataProviderFactory.GetProvider(targetCurrency).Returns(_dataProvider);
        
        var requestedCurrencies = new[] { new Currency("USD"), new Currency("JPY") };

        // Act
        var result = (await _service.GetExchangeRatesAsync(targetCurrency, requestedCurrencies, CancellationToken.None)).ToList();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(r => r.SourceCurrency.Code == "USD"));
            Assert.That(result.Any(r => r.SourceCurrency.Code == "JPY"));
            Assert.That(result.All(r => r.TargetCurrency.Code == "CZK"));
            _logger.VerifyLogDebug(1, "Fetching exchange rates for 2 requested currencies via provider CZK.");
            _logger.VerifyLogInformation(1, "Provider CZK returned 2/3 matching rates.");
        });
    }

    [Test]
    public async Task GetExchangeRatesAsync_EmptyRequestedCurrencies_ReturnsEmpty()
    {
        // Arrange
        var targetCurrency = "CZK";
        var allRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 22.5m),
            new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 24.0m)
        };
        
        _dataProvider.ExchangeRateProviderTargetCurrencyCode.Returns(targetCurrency);
        _dataProvider.GetDailyRatesAsync(Arg.Any<CancellationToken>()).Returns(allRates);
        _dataProviderFactory.GetProvider(targetCurrency).Returns(_dataProvider);
        
        var requestedCurrencies = Array.Empty<Currency>();

        // Act
        var result = (await _service.GetExchangeRatesAsync(targetCurrency, requestedCurrencies, CancellationToken.None)).ToList();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Empty);
            _logger.VerifyLogDebug(1, "Fetching exchange rates for 0 requested currencies via provider CZK.");
            _logger.VerifyLogInformation(1, "Provider CZK returned 0/2 matching rates.");
        });
    }

    [Test]
    public async Task GetExchangeRatesAsync_CaseInsensitiveMatching_ReturnsMatchingRates()
    {
        // Arrange
        var targetCurrency = "CZK";
        var allRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 22.5m),
            new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 24.0m)
        };
        
        _dataProvider.ExchangeRateProviderTargetCurrencyCode.Returns(targetCurrency);
        _dataProvider.GetDailyRatesAsync(Arg.Any<CancellationToken>()).Returns(allRates);
        _dataProviderFactory.GetProvider(targetCurrency).Returns(_dataProvider);
        
        // Request currencies with different casing
        var requestedCurrencies = new[] { new Currency("usd"), new Currency("Eur") };

        // Act
        var result = (await _service.GetExchangeRatesAsync(targetCurrency, requestedCurrencies, CancellationToken.None)).ToList();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(r => r.SourceCurrency.Code == "USD"));
            Assert.That(result.Any(r => r.SourceCurrency.Code == "EUR"));
            _logger.VerifyLogDebug(1, "Fetching exchange rates for 2 requested currencies via provider CZK.");
            _logger.VerifyLogInformation(1, "Provider CZK returned 2/2 matching rates.");
        });
    }

    [Test]
    public async Task GetExchangeRatesAsync_NoMatchingCurrencies_ReturnsEmpty()
    {
        // Arrange
        var targetCurrency = "CZK";
        var allRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 22.5m),
            new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 24.0m)
        };
        
        _dataProvider.ExchangeRateProviderTargetCurrencyCode.Returns(targetCurrency);
        _dataProvider.GetDailyRatesAsync(Arg.Any<CancellationToken>()).Returns(allRates);
        _dataProviderFactory.GetProvider(targetCurrency).Returns(_dataProvider);
        
        var requestedCurrencies = new[] { new Currency("GBP"), new Currency("CAD") };

        // Act
        var result = (await _service.GetExchangeRatesAsync(targetCurrency, requestedCurrencies, CancellationToken.None)).ToList();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Empty);
            _logger.VerifyLogDebug(1, "Fetching exchange rates for 2 requested currencies via provider CZK.");
            _logger.VerifyLogInformation(1, "Provider CZK returned 0/2 matching rates.");
        });
    }
}
