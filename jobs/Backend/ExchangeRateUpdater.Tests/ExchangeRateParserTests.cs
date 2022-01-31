using ExchangeRateUpdater.Sources;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateParserTests
    {
        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.ut.json")
                .Build();
            return config;
        }

        private readonly IConfiguration _configuration;
        public ExchangeRateParserTests()
        {
            _configuration = InitConfiguration();
        }
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void RegexShouldDisallowNegativeValues_Test()
        {
            var czParser = new ExchangeRateParserCzechBank(_configuration);
            Assert.False(czParser.ParseExchangeRate("Australia|dollar|1|AUD|-15.301").Success);
        }

        [Test]
        public void RegexShouldDisallowAmountLessThan1_Test()
        {
            var czParser = new ExchangeRateParserCzechBank(_configuration);
            Assert.False(czParser.ParseExchangeRate("Australia|dollar|-1|AUD|1.0").Success);
        }


        [Test]
        public void AmountRecalculationTest_Test()
        {
            var czParser = new ExchangeRateParserCzechBank(_configuration);
            Assert.AreEqual(czParser.ParseExchangeRate("Australia|dollar|100|AUD|66.6123456789").Rate.Value, 0.666123456789);
        }

        [Test]
        public void NoRecalculationOnAmountOne_Test()
        {
            var czParser = new ExchangeRateParserCzechBank(_configuration);
            Assert.False(czParser.ParseExchangeRate("Australia|dollar|1|AUD|-15.301").Success);
        }
    }
}