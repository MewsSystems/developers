using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Sources;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderTests
    {
        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.ut.json")
                .Build();
            return config;
        }

        private readonly IConfiguration _configuration;
        public ExchangeRateProviderTests()
        {
            _configuration = InitConfiguration();
        }
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task ShouldReturnUsdOnly_Test()
        {
            var data = await File.ReadAllTextAsync(Path.Combine("Resources", "CzechBankData.txt"));
            var czParser = new ExchangeRateParserCzechBank(_configuration);

            var czProvider = new ExchangeRateProviderCzechBank(_configuration, czParser);
            var rates = (await czProvider.GetExchangeRates(new Currency[] { new Currency("USD") }, data)).ToList();

            Assert.AreEqual(1, rates.Count);
            Assert.AreEqual("USD", rates[0].TargetCurrency.Code);
            Assert.AreEqual(21.940, rates[0].Value);

        }


    }
}