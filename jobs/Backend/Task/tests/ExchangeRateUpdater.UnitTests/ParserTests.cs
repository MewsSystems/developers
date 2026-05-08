using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Data.Helpers;
using ExchangeRateUpdater.BusinessLogic.Models;
using Xunit;

namespace ExchangeRateUpdater.UnitTests
{
    public class ParserTests
    {
        [Fact]
        public void ParseRates_ReturnsRatesForRequestedCurrencies()
        {
            var content = "14 Nov 2025\n#222\nCountry|Currency|Amount|Code|Rate\nUSA|dollar|1|USD|20.786\nJapan|yen|100|JPY|13.511\n";
            var currencies = new[] { new Currency("USD"), new Currency("CZK"), new Currency("JPY") };

            var rates = ParseHelper.ParseRates(content, currencies).ToList();

            Assert.Equal(2, rates.Count);

            var usd = rates.Single(r => r.SourceCurrency.Code == "USD");
            Assert.Equal("CZK", usd.TargetCurrency.Code);
            Assert.Equal(20.786m, usd.Value);

            var jpy = rates.Single(r => r.SourceCurrency.Code == "JPY");
            Assert.Equal("CZK", jpy.TargetCurrency.Code);
            Assert.Equal(13.511m / 100m, jpy.Value);
        }

        [Fact]
        public void ParseRates_IgnoresCurrenciesNotRequested()
        {
            var content = "14 Nov 2025\n#222\nCountry|Currency|Amount|Code|Rate\nUSA|dollar|1|USD|20.786\n";
            var currencies = new[] { new Currency("JPY"), new Currency("CZK") };

            var rates = ParseHelper.ParseRates(content, currencies);

            Assert.Empty(rates);
        }
    }
}
