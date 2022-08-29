using ExchangeRateUpdater;
using ExchangeRateUpdater.Helpers;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unit.Tests
{
    public class ExchangeRateProvidersTests
    {
        private Mock<IExchangeRateService> _exchangeRateService;
        private ExchangeRateProvider _sut;

        [SetUp]
        public void Setup()
        {
            _exchangeRateService = new Mock<IExchangeRateService>();
            _sut = new ExchangeRateProvider(_exchangeRateService.Object);
        }

        [Test]
        public async Task ShouldNot_Get_Exchange_Rates()
        {
           Given_A_Invalid_Exchange_Rate();

           var rates =  await _sut.GetExchangeRates(new List<Currency> { new Currency("AUD")});

           var result = rates.Any(x => x.TargetCurrency.Code == "CZK");
            
           Assert.That(result, Is.False);
        }

        [Test]
        public async Task Should_Get_Exchange_Rates()
        {
            Given_A_Valid_Exchange_Rate();

            var rates = await _sut.GetExchangeRates(new List<Currency> { new Currency("AUD") });

            var result = rates.Any(x => x.TargetCurrency.Code == "CZK");

            Assert.That(result, Is.True);
        }

        private void Given_A_Valid_Exchange_Rate()
        {
            _exchangeRateService.Setup(x => x.GetDailyExhangeRate()).ReturnsAsync(new List<CNBExchangeRateItem>{ new CNBExchangeRateItem {
                Amount = 1,
                Code = "AUD",
                Currency = "dollar",
                Country = "Australia",
                Rate = 16.927M
            }});
        }

        private void Given_A_Invalid_Exchange_Rate()
        {
            _exchangeRateService.Setup(x => x.GetDailyExhangeRate()).ReturnsAsync(new List<CNBExchangeRateItem>{ new CNBExchangeRateItem {
                Amount = 1,
                Code = "RUB",
                Currency = "ruble",
                Country = "Russia",
                Rate = 0.0M
            }});
        }

    }
}
