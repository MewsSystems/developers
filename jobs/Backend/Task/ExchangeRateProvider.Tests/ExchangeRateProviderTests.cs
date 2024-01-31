using ExchangeRateUpdater.Model;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using static ExchangeRateUpdater.Model.Constants;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderTests
    {
        private readonly AutoMocker _mocker;
        private readonly CancellationTokenSource _tokenSource;
        private readonly ExchangeRate[] _availableRates;

        public ExchangeRateProviderTests()
        {
            _mocker = new AutoMocker(MockBehavior.Strict);
            _tokenSource = new CancellationTokenSource();
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            _availableRates = new[] {
                new ExchangeRate(Currencies.CZK, Currencies.USD, 22m,today ),
                new ExchangeRate(Currencies.USD, Currencies.CZK, 1/22m,today),
                new ExchangeRate(Currencies.EUR, Currencies.USD, 23m,today),
                new ExchangeRate(Currencies.JPY, Currencies.PHP, 24m,today),
                new ExchangeRate(Currencies.PHP, Currencies.CZK, 25m, today),
                new ExchangeRate(Currencies.CZK, Currencies.AUD, 26m, today)
            };

            _mocker.GetMock<ICnbApiClient>().Setup(api => api.GetDailyRates(It.IsAny<CancellationToken>())).ReturnsAsync(_availableRates);
        }

        [Fact]
        public async void GetExchangeRates_returns_all_available_rates_for_all_currencies()
        {
            var allCurrencies = _availableRates.SelectMany(r => GetCurrencies(r));

            var exchangeRateProvider = _mocker.CreateInstance<ExchangeRateProvider>();

            var  rates = await exchangeRateProvider.GetExchangeRates(allCurrencies, _tokenSource.Token);

            rates.Should()
                .BeEquivalentTo(_availableRates);
        }


        [Fact]
        public async void GetExchangeRates_returns_only_requested_currencies()
        {
            IEnumerable<Currency> currencies = new[] { Currencies.USD, Currencies.EUR };

            var exchangeRateProvider = _mocker.CreateInstance<ExchangeRateProvider>();

            var rates = await exchangeRateProvider.GetExchangeRates(currencies, _tokenSource.Token);

            rates.Should()
                .BeEquivalentTo(_availableRates.Where(rate => rate.IsFromTo(Currencies.EUR, Currencies.USD)));
        }

        [Fact]
        public async void GetExchangeRates_does_not_return_missing_currencies()
        {
            IEnumerable<Currency> currencies = new[] { Currencies.USD, Currencies.THB };

            var exchangeRateProvider = _mocker.CreateInstance<ExchangeRateProvider>();

            var rates = await exchangeRateProvider.GetExchangeRates(currencies, _tokenSource.Token);

            rates.Should().BeEmpty();
        }

        [Fact]
        public async void GetExchangeRates_returns_only_existing_rates_without_calculating_reverse_direction()
        {
            IEnumerable<Currency> currencies = new[] { Currencies.USD, Currencies.EUR };

            var exchangeRateProvider = _mocker.CreateInstance<ExchangeRateProvider>();

            var rates = await exchangeRateProvider.GetExchangeRates(currencies, _tokenSource.Token);

            rates.Should()
                .BeEquivalentTo(_availableRates.Where(rate => rate.IsFromTo(Currencies.EUR, Currencies.USD)));
        }

        [Fact]
        public async void GetExchangeRates_returns_rates_for_both_directions_when_exist()
        {
            IEnumerable<Currency> currencies = new[] { Currencies.USD, Currencies.CZK };

            var exchangeRateProvider = _mocker.CreateInstance<ExchangeRateProvider>();

            var rates = await exchangeRateProvider.GetExchangeRates(currencies, _tokenSource.Token);

            rates
                .Should()
                .BeEquivalentTo(
                    _availableRates.Where(r =>
                        r.IsFromTo(Currencies.USD, Currencies.CZK) || r.IsFromTo(Currencies.CZK, Currencies.USD)));
        }

        private static IEnumerable<Currency> GetCurrencies(ExchangeRate rate) {
            yield return rate.SourceCurrency;
            yield return rate.TargetCurrency;
        }

    }
}