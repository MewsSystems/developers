using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using NUnit.Framework;
using Shouldly;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;

namespace ExchangeRateUpdater.Tests.Services
{
    public class CurrencyLoaderTests
    {
        private ICurrencyLoader _currencyLoader;
        private IConfiguration _config;

        [SetUp]
        public void Setup()
        {
            var configBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(new[] { new KeyValuePair<string, string>("CurrenciesFilePath", "testCurrencies.json") })
                .Build();

            _config = configBuilder;
            _currencyLoader = new CurrencyLoader(_config);
        }

        [Test]
        public void LoadCurrencies_ReturnsCorrectNumberOfCurrencies()
        {
            var currencies = _currencyLoader.LoadCurrencies();

            
            currencies.Count().ShouldBe(3);
        }
        //I would add more tests but just showcasing how id approach em

    }
}
