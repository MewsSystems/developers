using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.DataSource;
using ExchangeRateUpdater.DTO;
using ExchangeRateUpdater.Provider;
using NSubstitute;
using NUnit.Framework;
using Unity;

namespace ExchangeRateUpdater.UnitTests
{
    [TestFixture]
    public class ExchangeRateProviderTests
    {
        private readonly IUnityContainer _unity;
        
        public ExchangeRateProviderTests()
        {
            _unity = UnityContainerFactory.Create(null);
        }

        [Test]
        public void ShouldProperlyUsedProvidedDataSource()
        {
            var dataSourceProvider = Substitute.For<IExchangeRateDataSourceProvider>();
            dataSourceProvider.Get().Returns(TestRates);
            _unity.RegisterInstance(dataSourceProvider);

            var exchangeRatesProvider = _unity.Resolve<IExchangeRateProvider>();

            var result = exchangeRatesProvider.GetExchangeRates(new List<Currency>
            {
                new Currency("NOK"),
                new Currency("CZK"),
                new Currency("XXX")
            }).ToList();

            Assert.That(result.Count, Is.EqualTo(1));

            var aud = result.FirstOrDefault();
            Assert.That(aud, Is.Not.Null);
            Assert.That(aud?.SourceCurrency.Code, Is.EqualTo("NOK"));
            Assert.That(aud?.TargetCurrency.Code, Is.EqualTo("CZK"));
            Assert.That(aud?.Value, Is.EqualTo(2.425m));
        }

        private const string TestRates = "22.02.2022 #37\r\nzemě|měna|množství|kód|kurz\r\nAustrálie|dolar|1|AUD|15,569\r\nBrazílie|real|1|BRL|4,251\r\nBulharsko|lev|1|BGN|12,526\r\nČína|žen-min-pi|1|CNY|3,413\r\nDánsko|koruna|1|DKK|3,293\r\nEMU|euro|1|EUR|24,500\r\nFilipíny|peso|100|PHP|42,084\r\nHongkong|dolar|1|HKD|2,768\r\nChorvatsko|kuna|1|HRK|3,251\r\nIndie|rupie|100|INR|28,897\r\nIndonesie|rupie|1000|IDR|1,504\r\nIsland|koruna|100|ISK|17,351\r\nIzrael|nový šekel|1|ILS|6,699\r\nJaponsko|jen|100|JPY|18,764\r\nJižní Afrika|rand|1|ZAR|1,428\r\nKanada|dolar|1|CAD|16,963\r\nKorejská republika|won|100|KRW|1,810\r\nMaďarsko|forint|100|HUF|6,884\r\nMalajsie|ringgit|1|MYR|5,160\r\nMexiko|peso|1|MXN|1,064\r\nMMF|ZPČ|1|XDR|30,305\r\nNorsko|koruna|1|NOK|2,425\r\nNový Zéland|dolar|1|NZD|14,543\r\nPolsko|zlotý|1|PLN|5,391\r\nRumunsko|leu|1|RON|4,953\r\nRusko|rubl|100|RUB|27,268\r\nSingapur|dolar|1|SGD|16,047\r\nŠvédsko|koruna|1|SEK|2,311\r\nŠvýcarsko|frank|1|CHF|23,502\r\nThajsko|baht|100|THB|66,671\r\nTurecko|lira|1|TRY|1,560\r\nUSA|dolar|1|USD|21,596\r\nVelká Británie|libra|1|GBP|29,274";
    }
}
