using AutoFixture;
using AutoFixture.AutoNSubstitute;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Models.Enums;
using ExchangeRateUpdater.Infrastructure.Clients;
using ExchangeRateUpdater.Infrastructure.Models.CzechNationalBank;
using ExchangeRateUpdater.Infrastructure.Providers;
using ExchangeRateUpdater.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace ExchangeRateUpdater.Infrastructure.UnitTests.Providers
{
    public class CzechNationalBankExchangeRateProviderTests
    {
        private readonly IFixture _fixture = new Fixture().Customize(new AutoNSubstituteCustomization() { ConfigureMembers = true });

        private readonly ICzechNationalBankApiClient _apiClient = Substitute.For<ICzechNationalBankApiClient>();
        private readonly ICache _cache = Substitute.For<ICache>();
        private readonly ILogger<CzechNationalBankExchangeRateProvider> _logger = Substitute.For<ILogger<CzechNationalBankExchangeRateProvider>>();

        private readonly CzechNationalBankExchangeRateProvider _subjectUnderTest;

        public CzechNationalBankExchangeRateProviderTests()
        {
            _subjectUnderTest = new CzechNationalBankExchangeRateProvider(_apiClient, _cache, _logger);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_WhenNoCachedValue_CallsApiClientAndSetCache()
        {
            // Arrange
            var targetCurrency = CurrencyCode.USD;
            var currencies = _fixture.Create<IEnumerable<Currency>>();
            var apiResponse = new CzechNationalBankExchangeRatesResponse()
            {
                Rates = new List<CzechNationalBankExchangeRate>
                {
                    new CzechNationalBankExchangeRate
                    {
                        CurrencyCode = "CZK",
                        Amount = 100,
                        Rate = 15.5m,
                        ValidFor = DateTime.Now.Date.ToString("yyyy-MM-dd")
                    }
                }
            };

            _cache.Get<CzechNationalBankExchangeRatesResponse>(Arg.Any<string>())
                .Returns(default(CzechNationalBankExchangeRatesResponse));

            _apiClient.GetExchangeRatesAsync(Arg.Any<DateTime>(), Arg.Any<string>())
                .Returns(apiResponse);

            // Act
            await _subjectUnderTest.GetExchangeRatesAsync(currencies, targetCurrency);

            // Assert
            await _apiClient.Received(1).GetExchangeRatesAsync(Arg.Any<DateTime>(), Arg.Any<string>());
            _cache.Received(1).Set(Arg.Any<string>(), apiResponse, Arg.Any<DateTimeOffset>());
        }

        [Fact]
        public async Task GetExchangeRatesAsync_WhenThereIsCachedValue_DoNotCallApi()
        {
            // Arrange
            var targetCurrency = CurrencyCode.USD;
            var currencies = _fixture.Create<IEnumerable<Currency>>();
            var apiResponse = new CzechNationalBankExchangeRatesResponse()
            {
                Rates = new List<CzechNationalBankExchangeRate>
                {
                    new()
                    {
                        CurrencyCode = "CZK",
                        Amount = 100,
                        Rate = 15.5m,
                        ValidFor = DateTime.Now.Date.ToString("yyyy-MM-dd")
                    }
                }
            };

            _cache.Get<CzechNationalBankExchangeRatesResponse>(Arg.Any<string>())
                .Returns(apiResponse);

            // Act
            await _subjectUnderTest.GetExchangeRatesAsync(currencies, targetCurrency);

            // Assert
            await _apiClient.DidNotReceive().GetExchangeRatesAsync(Arg.Any<DateTime>(), Arg.Any<string>());
        }

        [Fact]
        public async Task GetExchangeRatesAsync_MapExchangeRateCorrectly()
        {
            // Arrange
            var targetCurrency = CurrencyCode.USD;
            var currencies = _fixture.Create<IEnumerable<Currency>>();
            var currencyCodes = currencies.Select(x => x.Code);
            var rate = new CzechNationalBankExchangeRate
            {
                CurrencyCode = "CZK",
                Amount = 100,
                Rate = 15.5m,
                ValidFor = DateTime.Now.Date.ToString("yyyy-MM-dd")
            };

            var apiResponse = new CzechNationalBankExchangeRatesResponse()
            {
                Rates = new List<CzechNationalBankExchangeRate>
                {
                    rate
                }
            };

            _cache.Get<CzechNationalBankExchangeRatesResponse>(Arg.Any<string>())
                .Returns(apiResponse);

            // Act
            var rates = await _subjectUnderTest.GetExchangeRatesAsync(currencies, targetCurrency);

            // Assert
            Assert.All(rates, x => Assert.Contains(x.SourceCurrency.Code, currencyCodes));
            Assert.All(rates, x => Assert.Equal(targetCurrency, x.TargetCurrency.Code));
            Assert.All(rates, x => Assert.Equal(Math.Round(rate.Rate / rate.Amount, 2), x.Value));
        }
    }
}
