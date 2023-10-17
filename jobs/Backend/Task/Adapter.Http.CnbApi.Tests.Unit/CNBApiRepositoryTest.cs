using System.ComponentModel.DataAnnotations;
using Adapter.Http.CnbApi.DTO;
using Adapter.Http.CnbApi.Repository;
using Adapter.Http.CnbApi.Settings;
using ExchangeRateUpdater.Domain.Entities;
using Flurl.Http;
using Flurl.Http.Testing;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Adapter.Http.CnbApi.Tests.Unit
{
    [TestFixture]
    internal class CNBApiRepositoryTest
    {
        private HttpTest? _httpTest;
        private CnbExchangeRateRepository _sut;

        private static readonly List<CurrencyCode> _currencies = new()
        {
                                                            new("EUR"),
                                                            new("USD"),
                                                            new("GBP")
                                                        };
        private const string _defaultCurrency = "CZK";

        [SetUp]
        public void Setup()
        {
            _httpTest = new HttpTest();
            _sut = new(new ExchangeRateSettings { ApiUrl = "http://fake/", Currencies = new string[] { }, DefaultCurrency = ""}, new Logger<CnbExchangeRateRepository>(new LoggerFactory()));
        }

        [Test]
        public async Task GetExchangeRates_Success()
        {
            //arrange
            _httpTest.RespondWithJson(new ExchangeRatesResponseDto
            {
                Rates = new List<ExchangeRateResponseDto>
                {
                    new()
                    {
                        Amount = 1,
                        CurrencyCode = "EUR",
                        Rate = 25.5m
                    }
                }
            });

            //act
            var response = await _sut.GetExchangeRates(_defaultCurrency, _currencies);

            //assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetExchangeRates_Correct_Conversion()
        {
            //arrange
            _httpTest.RespondWithJson(new ExchangeRatesResponseDto
            {
                Rates = new List<ExchangeRateResponseDto>
                {
                    new()
                    {
                        Amount = 1,
                        CurrencyCode = "EUR",
                        Rate = 25.5m
                    }
                }
            });

            //act
            var response = await _sut.GetExchangeRates(_defaultCurrency, _currencies);

            //assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Count, Is.EqualTo(1));
            Assert.That(response.FirstOrDefault().ToString(), Is.EqualTo("EUR/CZK=25.5"));
        }

        [Test]
        public async Task GetExchangeRates_Show_Valid_Currencies_Only()
        {
            //arrange
            _httpTest.RespondWithJson(new ExchangeRatesResponseDto
            {
                Rates = new List<ExchangeRateResponseDto>
                {
                    new()
                    {
                        Amount = 1,
                        CurrencyCode = "EUR",
                        Rate = 25.5m
                    },
                    new()
                    {
                        Amount = 1,
                        CurrencyCode = "USD",
                        Rate = 0
                    }
                }
            });


            //act
            var response = await _sut.GetExchangeRates(_defaultCurrency, _currencies);

            //assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Count, Is.EqualTo(2));
            Assert.That(_currencies.Count, Is.EqualTo(3));
        }

        [Test]
        public async Task GetExchangeRates_Success_Mapping()
        {
            //arrange
            _httpTest.RespondWithJson(new ExchangeRatesResponseDto
            {
                Rates = new List<ExchangeRateResponseDto>
                {
                    new()
                    {
                        Amount = 1,
                        CurrencyCode = "EUR",
                        Rate = 25.5m
                    }
                }
            });

            //act
            var response = await _sut.GetExchangeRates(_defaultCurrency, _currencies);

            //assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Count, Is.EqualTo(1));
            Assert.IsInstanceOf(typeof(IEnumerable<ExchangeRate>), response);
        }

        [Test]
        public Task GetExchangeRates_Server_Error()
        {
            //arrange
            _httpTest.RespondWithJson(new { message = "server error" }, 500);

            //act & assert
            _ = Assert.ThrowsAsync<FlurlHttpException>(async () => await _sut.GetExchangeRates(_defaultCurrency, _currencies));
            return Task.CompletedTask;
        }

        [Test]
        public Task Execute_No_Rates_Found()
        {
            //arrange
            _httpTest.RespondWithJson(new ExchangeRatesResponseDto
            {
                Rates = new List<ExchangeRateResponseDto>()
            });

            //act & assert
            _ = Assert.ThrowsAsync<ValidationException>(async () => await _sut.GetExchangeRates(_defaultCurrency, _currencies));
            return Task.CompletedTask;
        }

        [Test]
        [TestCase("xx")]
        [TestCase("xxxx")]
        [TestCase("")]
        [TestCase(" ")]
        [Parallelizable(ParallelScope.All)]
        public Task Execute_With_Invalid_Currency(string currency)
        {
            //act & assert
            _ = Assert.ThrowsAsync<ArgumentException>(() =>
            {
                _currencies.Add(new CurrencyCode(currency));
                return Task.CompletedTask;
            });
            return Task.CompletedTask;
        }

        [TearDown]
        public void DisposeHttpTest()
        {
            _httpTest.Dispose();
        }
    }
}
