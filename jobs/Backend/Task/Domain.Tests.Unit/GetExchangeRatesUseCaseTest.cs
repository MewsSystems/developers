using System.ComponentModel.DataAnnotations;
using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Ports;
using ExchangeRateUpdater.Domain.UseCase;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Domain.Tests.Unit
{
    [TestFixture]
    internal class GetExchangeRatesUseCaseTest
    {
        private GetExchangeRatesUseCase _sut;
        private Mock<IExchangeRateRepository> _cnBApiRepositoryMock;

        readonly IEnumerable<CurrencyCode> _currencies = new List<CurrencyCode>
                                            {
                                                new("EUR"),
                                                new("USD"),
                                                new("GBP")
                                            };

        private const string _defaultCurrency = "CZK";

        [SetUp]
        public void Setup()
        {
            _cnBApiRepositoryMock = new Mock<IExchangeRateRepository>();
            _sut = new GetExchangeRatesUseCase(_cnBApiRepositoryMock.Object, new Logger<GetExchangeRatesUseCase>(new LoggerFactory()));
        }

        [Test]
        public async Task Execute_Success()
        {
            //arrange
            var list = new List<ExchangeRate> { new(new CurrencyCode("EUR"), new CurrencyCode(_defaultCurrency), 50) };
            _cnBApiRepositoryMock.Setup(x => x.GetExchangeRates(_defaultCurrency, _currencies)).ReturnsAsync(list);

            //act
            var response = await _sut.Execute(_defaultCurrency, _currencies);

            //assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task Execute_Show_Valid_Currencies_Only()
        {
            //arrange
            var list = new List<ExchangeRate>
            {
                new(new CurrencyCode("EUR"), new CurrencyCode(_defaultCurrency), 50),
                new(new CurrencyCode("USD"), new CurrencyCode(_defaultCurrency), 100)
            };
            _cnBApiRepositoryMock.Setup(x => x.GetExchangeRates(_defaultCurrency, _currencies)).ReturnsAsync(list);

            //act
            var response = await _sut.Execute(_defaultCurrency, _currencies);

            //assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Count, Is.EqualTo(2));
            Assert.That(_currencies.Count, Is.EqualTo(3));
            Assert.That(response.FirstOrDefault().ToString(), Is.EqualTo("EUR/CZK=50"));
            Assert.That(response.LastOrDefault().ToString(), Is.EqualTo("USD/CZK=100"));
        }

        [Test]
        public async Task Execute_Check_Correct_Exchange_Computation()
        {
            //arrange
            var list = new List<ExchangeRate> { new(new CurrencyCode("EUR"), new CurrencyCode(_defaultCurrency), 50) };
            _cnBApiRepositoryMock.Setup(x => x.GetExchangeRates(_defaultCurrency, _currencies)).ReturnsAsync(list);

            //act
            var response = await _sut.Execute(_defaultCurrency, _currencies);

            //assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Count, Is.EqualTo(1));
            Assert.That(response.FirstOrDefault().ToString(), Is.EqualTo("EUR/CZK=50"));
        }


        [Test]
        public Task Execute_No_Valid_Exchange_Rates_Found()
        {
            //arrange
            _cnBApiRepositoryMock.Setup(x => x.GetExchangeRates(_defaultCurrency, _currencies)).ReturnsAsync(new List<ExchangeRate>());

            //act & assert
            _ = Assert.ThrowsAsync<ValidationException>(async () => await _sut.Execute(_defaultCurrency, _currencies));
            return Task.CompletedTask;
        }
    }
}
