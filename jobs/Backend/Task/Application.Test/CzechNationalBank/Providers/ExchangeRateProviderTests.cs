using Application.Common.Models;
using Application.Common.Validations;
using Application.CzechNationalBank.ApiClient;
using Application.CzechNationalBank.ApiClient.Dtos;
using Application.CzechNationalBank.Mappings;
using Application.CzechNationalBank.Providers;

using FakeItEasy;

using FluentAssertions;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Application.Test.CzechNationalBank.Providers
{
    public class ExchangeRateProviderTests
    {
        private readonly ICNBClient cNBClient;
        private readonly IExchangeRateProvider exchangeRateProvider;
        private readonly ILogger<ExchangeRateProvider> logger;

        public ExchangeRateProviderTests()
        {
            cNBClient = A.Fake<ICNBClient>();
            logger = A.Fake<ILogger<ExchangeRateProvider>>();
            var currencyValidator = new CurrencyValidator();
            var mapper = new CNBExchangeRateMappingService(A.Fake<ILogger<CNBExchangeRateMappingService>>());
            exchangeRateProvider = new ExchangeRateProvider(cNBClient, logger, currencyValidator, mapper);
        }

        [Fact]
        public async Task GetExchangeRates_WithValidInput_ReturnsRates()
        {
            A.CallTo(() => cNBClient.GetExRateDailies()).Returns(new CNBExRateDailyResponseDto()
            {
                Rates = [ 
                    new CNBExRateDailyRestDto() {
                    Amount = 1,
                    Country = "Australia",
                    Currency = "dollar",
                    CurrencyCode = "AUD",
                    Order = 110,
                    Rate = 15.0354m,
                    ValidFor = new DateTime(2024, 6, 7)
                }, new CNBExRateDailyRestDto() {
                    Amount = 100,
                    Country = "Hungary",
                    Currency = "forint",
                    CurrencyCode = "HUF",
                    Order = 110,
                    Rate = 6.32m,
                    ValidFor = new DateTime(2024, 6, 7)
                }]
            });

            var currencies = new List<Currency>
            {
                new Currency("AUD"),
                new Currency("USD"),
                new Currency("HUF")
            };

            var res = await exchangeRateProvider.GetExchangeRates(currencies);

            A.CallTo(() => cNBClient.GetExRateDailies()).MustHaveHappenedOnceExactly();
            res.Should().BeEquivalentTo(new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("AUD"), new Currency("CZK"), 15.0354m),
                new ExchangeRate(new Currency("HUF"), new Currency("CZK"), 0.0632m)
            });
        }
    }
}
