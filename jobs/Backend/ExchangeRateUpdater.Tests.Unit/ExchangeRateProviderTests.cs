using AutoFixture;
using ExchangeRateUpdater.CzechNationalBank;
using ExchangeRateUpdater.CzechNationalBank.Contracts;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace ExchangeRateUpdater.Tests.Unit
{
    public class ExchangeRateProviderTests
    {
        private readonly IFixture _fixture = new Fixture();

        private ICzechNationalBankClient _client = default!;
        private ExchangeRateProvider _sut = default!;

        protected virtual void InitializeSut()
        {
            _client = Substitute.For<ICzechNationalBankClient>();
            _sut = new ExchangeRateProvider(_client);
        }

        [Fact]
        public async Task GetExchangeRates_ShouldOnlyReturnRequestedCurrencies()
        {
            // arrange.            
            InitializeSut();

            var currencies = new List<Currency>
            {
                 new("EUR"),
                 new("JPY"),
            };

            _client.GetDailyExchangeRatesAsync()
                .Returns(Task.FromResult(
                    new GetDailyExchangeRatesResponse
                    {
                        Rates = new List<DailyExchangeRate>
                         {
                             CreateRate("EUR"),
                             CreateRate("INR"),
                             CreateRate("JPY"),
                         }
                    }));

            // act.
            IEnumerable<ExchangeRate> exchangeRates = await _sut.GetExchangeRatesAsync(currencies);

            //assert.
            Assert.Equal(2, exchangeRates.Count());
            Assert.Contains(exchangeRates, x => x.SourceCurrency.Code == "EUR");
            Assert.Contains(exchangeRates, x => x.SourceCurrency.Code == "JPY");
            Assert.DoesNotContain(exchangeRates, x => x.SourceCurrency.Code == "INR");
        }

        [Fact]
        public async Task GetExchangeRates_WhenNoCurrenciesSupplied_ShouldNotCallClient()
        {
            // arrange.            
            InitializeSut();

            var currencies = new List<Currency>();

            // act.
            IEnumerable<ExchangeRate> exchangeRates = await _sut.GetExchangeRatesAsync(currencies);

            //assert.
            Assert.Empty(exchangeRates);

            await _client.DidNotReceive().GetDailyExchangeRatesAsync(Arg.Any<Language>());
        }

        [Fact]
        public async Task GetExchangeRates_WhenClientHasAnException_ThenExceptionShouldBeThrown()
        {
            // arrange.            
            InitializeSut();

            var currencies = _fixture.Create<List<Currency>>();

            _client.GetDailyExchangeRatesAsync(Arg.Any<Language>()).Throws(new Exception("Server Error"));

            // act & assert
            await Assert.ThrowsAsync<Exception>(() => _sut.GetExchangeRatesAsync(currencies));
        }

        [Fact]
        public async Task GetExchangeRates_ShouldMapSourceCurrency()
        {
            // arrange.            
            InitializeSut();

            var currencies = new List<Currency>
            {
                 new("EUR"),
            };

            _client.GetDailyExchangeRatesAsync()
                .Returns(Task.FromResult(
                    new GetDailyExchangeRatesResponse
                    {
                        Rates = new List<DailyExchangeRate>
                        {
                             CreateRate("EUR")
                        }
                    }));

            // act.
            IEnumerable<ExchangeRate> exchangeRates = await _sut.GetExchangeRatesAsync(currencies);

            //assert.
            ExchangeRate exchangeRate = exchangeRates.First();

            Assert.Equal("EUR", exchangeRate.SourceCurrency.Code);
        }

        [Fact]
        public async Task GetExchangeRates_ShouldMapTargetCurrency()
        {
            // arrange.            
            InitializeSut();

            var currencies = new List<Currency>
            {
                 new("EUR"),
            };

            _client.GetDailyExchangeRatesAsync()
                .Returns(Task.FromResult(
                    new GetDailyExchangeRatesResponse
                    {
                        Rates = new List<DailyExchangeRate>
                        {
                             CreateRate("EUR")
                        }
                    }));

            // act.
            IEnumerable<ExchangeRate> exchangeRates = await _sut.GetExchangeRatesAsync(currencies);

            //assert.
            ExchangeRate exchangeRate = exchangeRates.First();

            Assert.Equal(_sut.TargetCurrency.Code, exchangeRate.TargetCurrency.Code);
        }

        [Fact]
        public async Task GetExchangeRates_ShouldMapRateValue()
        {
            // arrange.            
            InitializeSut();

            var expectedExchangeRate = _fixture.Create<DailyExchangeRate>();
            var expectedRate = (expectedExchangeRate.Rate / expectedExchangeRate.Amount);

            var currencies = new List<Currency>
            {
                 new(expectedExchangeRate.CurrencyCode),
            };

            _client.GetDailyExchangeRatesAsync()
                .Returns(Task.FromResult(
                    new GetDailyExchangeRatesResponse
                    {
                        Rates = new List<DailyExchangeRate>
                        {
                             expectedExchangeRate
                        }
                    }));

            // act.
            IEnumerable<ExchangeRate> exchangeRates = await _sut.GetExchangeRatesAsync(currencies);

            //assert.
            ExchangeRate exchangeRate = exchangeRates.First();

            Assert.Equal(expectedRate, exchangeRate.Value);
        }

        private DailyExchangeRate CreateRate(string currencyCode)
        {
            return _fixture.Build<DailyExchangeRate>()
                .With(x => x.CurrencyCode, currencyCode)
                .Create();
        }
    }
}