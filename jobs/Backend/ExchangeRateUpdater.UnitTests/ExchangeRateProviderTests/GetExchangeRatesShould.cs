using Castle.Core.Logging;
using CzechNationalBankApi;
using FluentAssertions;
using ExchangeRateUpdater.Application;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace ExchangeRateUpdater.UnitTests.ExchangeRateProviderTests
{
    public class GetExchangeRatesShould
    {
        private ILogger<ExchangeRateProvider> _logger = Substitute.For<ILogger<ExchangeRateProvider>>();
        private ICzechBankApiService _czechBankApiService = Substitute.For<ICzechBankApiService>();

        private ExchangeRateProvider _exchangeRateProvider;

        public GetExchangeRatesShould()
        {
            _exchangeRateProvider = new ExchangeRateProvider(_logger, _czechBankApiService);
        }

        [Fact]
        public async Task ReturnEmptyListIfCurrenciesProvidedEmpty()
        {
            var expected = Enumerable.Empty<ExchangeRate>();

            var actual = await _exchangeRateProvider.GetExchangeRatesAsync(Enumerable.Empty<Currency>());

            await _czechBankApiService.DidNotReceive().GetExchangeRatesAsync();

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ReturnEmptyListIfNoResults()
        {
            var expected = Enumerable.Empty<ExchangeRate>();

            _czechBankApiService.GetExchangeRatesAsync().Returns(Enumerable.Empty<CzechExchangeItemDto>());

            var actual = await _exchangeRateProvider.GetExchangeRatesAsync(Enumerable.Empty<Currency>());

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ReturnExpectedCurrencies_OnlyIfRequested_CaseInsensitive()
        {
            var dummyCurrencies = new[] {
                new Currency("AAA"),
                new Currency("BOB")
            };

            var dummyResponse = new[] { 
                new CzechExchangeItemDto { Amount = 1, Code = "AaA", Country = "An Awesome Alliance", Currency = "awesome", Rate = 22.33m },
                new CzechExchangeItemDto { Amount = 100, Code = "BOB", Country = "Bob country", Currency = "bobcoin", Rate = 12m },
                new CzechExchangeItemDto { Amount = 999, Code = "ZZZ", Country = "Sleeping country", Currency = "sleeps", Rate = 129919m },
            };

            var expected = new[] {
                new ExchangeRate(new Currency("AAA"), new Currency("CZK"), 22.33m),
                new ExchangeRate(new Currency("BOB"), new Currency("CZK"), 12m),
            };

            _czechBankApiService.GetExchangeRatesAsync().Returns(dummyResponse);

            var actual = await _exchangeRateProvider.GetExchangeRatesAsync(dummyCurrencies);
            
            actual.Count().Should().Be(expected.Length);
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task NotSwallowException()
        {
            _czechBankApiService.GetExchangeRatesAsync().ThrowsAsync(new Exception("dummy-exception"));

            var actual = async () => await _exchangeRateProvider.GetExchangeRatesAsync([new Currency("DAN")]);

            await actual.Should().ThrowExactlyAsync<Exception>();
        }
    }
}