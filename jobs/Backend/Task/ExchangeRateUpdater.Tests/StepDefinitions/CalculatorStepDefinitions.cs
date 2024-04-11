using AutoFixture;
using Castle.Core.Logging;
using ExchangeRateUpdater.Services.Client;
using ExchangeRateUpdater.Services.Client.ClientModel;
using ExchangeRateUpdater.Services.Configuration;
using ExchangeRateUpdater.Services.Domain;
using ExchangeRateUpdater.Services.Implementations;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Refit;

namespace ExchangeRateUpdater.Tests.StepDefinitions
{
    [Binding]
    internal sealed class ExchangeRateProviderStepDefinitions
    {
        private readonly Mock<ICzechNationalBankClient> _apiClient = new();
        private readonly Mock<IMemoryCache> _cache = new();
        private readonly Mock<ILogger<ExchangeRateProvider>> _logger = new();
        private readonly AppConfiguration _configuration = new();
        private Exception _exception;
        private IEnumerable<ExchangeRate> _expectedReturn;

        internal ExchangeRateProviderStepDefinitions()
        {
            _cache.Setup(c => c.CreateEntry(It.IsAny<string>())).Returns(new Mock<ICacheEntry>().Object);
        }

        [Given("ApiClient returns rates for the currencies '(.*)'")]
        internal void GivenAPIReturnsRatesForCurrencies(List<string> currencies)
        {
            var fixture = new Fixture();
            var rates = new List<ExchangeRateResponse>();
            var apiResponse = new ExchangeRateResponseList
            {
                Rates = rates
            };

            currencies.ForEach(currency =>
            {
                rates.Add(
                    fixture.Build<ExchangeRateResponse>()
                           .With(x => x.CurrencyCode, currency)
                           .Create());
            });

            _apiClient.Setup(x => x.GetDailyRatesAsync("EN"))
                      .ReturnsAsync(apiResponse);
        }

        delegate void OutDelegate<TIn, TOut>(TIn input, out TOut output);

        [Given("Cache returns rates for the currencies '(.*)'")]
        internal void GivenCacheReturnsRatesForCurrencies(List<string> currencies)
        {
            var fixture = new Fixture();
            var rates = new List<ExchangeRateResponse>();
            var apiResponse = new ExchangeRateResponseList
            {
                Rates = rates
            };

            currencies.ForEach(currency =>
            {
                rates.Add(
                    fixture.Build<ExchangeRateResponse>()
                           .With(x => x.CurrencyCode, currency)
                           .Create());
            });


            _cache.Setup(c => c.TryGetValue("DailyRatesShortTerm", out It.Ref<object>.IsAny))
                .Callback(new OutDelegate<object, object>((object _, out object v) =>
                    v = apiResponse))
                .Returns(true);
        }

        [Given("Short term cache is empty")]
        internal void ShortTermCacheIsEmpty()
        {
            object? expected = null;
            _cache.Setup(c => c.TryGetValue("DailyRatesShortTerm", out expected)).Returns(false);
        }

        [Given("API has issues")]
        internal void APIReturnsError()
        {
            _apiClient.Setup(x => x.GetDailyRatesAsync("EN"))
                      .Throws(new Exception("Api thrown an exception"));
        }

        [Given("Long term cache returns rates for the currencies '(.*)'")]
        internal void GivenLongTermCacheReturnsRatesForCurrencies(List<string> currencies)
        {
            var fixture = new Fixture();
            var rates = new List<ExchangeRateResponse>();
            var apiResponse = new ExchangeRateResponseList
            {
                Rates = rates
            };

            currencies.ForEach(currency =>
            {
                rates.Add(
                    fixture.Build<ExchangeRateResponse>()
                           .With(x => x.CurrencyCode, currency)
                           .Create());
            });


            _cache.Setup(c => c.TryGetValue("DailyRatesLongTerm", out It.Ref<object>.IsAny))
                .Callback(new OutDelegate<object, object>((object _, out object v) =>
                    v = apiResponse))
                .Returns(true);
        }

        [Given("Default culture is EN")]
        internal void GivenCultureEN()
        {
            _configuration.Culture = "EN";
        }

        [Given("Cache is empty")]
        internal void CacheIsEmpty()
        {
            object? expected = null;
            _cache.Setup(c => c.TryGetValue(It.IsAny<string>(), out expected)).Returns(false);
        }

        [When("Getting the rates for the currencies '(.*)'")]
        internal async Task CallSut(List<string> currencies)
        {
            try
            {
                var sut = new ExchangeRateProvider(
                    _logger.Object,
                    _configuration,
                    _apiClient.Object,
                    _cache.Object);

                _expectedReturn =  await sut.GetExchangeRates(currencies.Select(c => (Currency)c));
            }
            catch(Exception ex)
            {
                _exception = ex;
            }
        }

        [Then("The rates for the following currencies should be returned '(.*)'")]
        internal void ShouldReturnRatesForSelectedCurrencies(List<string> currencies)
        {
            _exception.Should().BeNull();
            _expectedReturn.Should().NotBeEmpty();
            _expectedReturn.Should().HaveCount(currencies.Count);
            _expectedReturn.Select(r => r.SourceCurrency.Code).Should().BeEquivalentTo(currencies);
        }

        [Then("Cache gets populated")]
        internal void CacheGetsPopulated()
        {
            _cache.Verify(c => c.CreateEntry("DailyRatesShortTerm"), Times.Once);
            _cache.Verify(c => c.CreateEntry("DailyRatesLongTerm"), Times.Once);
        }

        [Then("API should not be consumed")]
        internal void APIShouldNotBeConsumed()
        {
            _apiClient.Verify(a => a.GetDailyRatesAsync(It.IsAny<string>()), Times.Never);
        }

        [Then("Long term cache should be called")]
        internal void LongTermCacheShouldBeCalled()
        {
            _cache.Verify(x => x.TryGetValue("DailyRatesLongTerm", out It.Ref<object>.IsAny), Times.Once);
        }

        [StepArgumentTransformation]
        internal List<string> TransformToListOfString(string commaSeparatedList)
        {
            return commaSeparatedList.Split(',').ToList();
        }
    }
}
