using System;
using System.Collections.Generic;
using ExchangeRateUpdater;
using ExchangeRateUpdater.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeRateUpdaterTests
{

    [TestClass]
    public class CnbParserTest
    {
        [TestMethod]
        public void ParseTest_From0626_Ok()
        {
            IParser parser = new CnbParser();
            Dictionary<string, decimal> currencies = parser.Parse(Constants.Source1);
            TestValue(currencies, "AUD", 15.658M); // Austrálie - dolar (1)
            TestValue(currencies, "BRL", 5.842M); // Brazílie - real (1)
            TestValue(currencies, "BGN", 13.030M); // Bulharsko - lev (1)
            TestValue(currencies, "CNY", 3.262M); // Čína - žen-min-pi (1) 
            TestValue(currencies, "DKK", 3.414M); // Dánsko - koruna (1)
            TestValue(currencies, "EUR", 25.485M); // EMU - euro (1)
            TestValue(currencies, "PHP", 0.43598M); // Filipíny - peso (100)
            TestValue(currencies, "HKD", 2.873M); // Hongkong - dolar (1)
            TestValue(currencies, "HRK", 3.446M); // Chorvatsko - kuna (1)
            TestValue(currencies, "INR", 0.32440M); // Indie - rupie (100)
            TestValue(currencies, "IDR", 0.001583M); // Indonesie - rupie (1000)
            TestValue(currencies, "ISK", 0.18011M); // Island - koruna (100)
            TestValue(currencies, "ILS", 6.245M); // Izrael - nový šekel (1)
            TestValue(currencies, "JPY", 0.20821M); // Japonsko - jen (100)
            TestValue(currencies, "ZAR", 1.565M); // Jižní Afrika - rand (1)
            TestValue(currencies, "CAD", 17.054M); // Kanada - dolar (1)
            TestValue(currencies, "KRW", 0.01942M); // Korejská republika - won (100)
            TestValue(currencies, "HUF", 0.07877M); // Maďarsko - forint (100)
            TestValue(currencies, "MYR", 5.409M); // Malajsie - ringgit (1)
            TestValue(currencies, "MXN", 1.169M); // Mexiko - peso (1)
            TestValue(currencies, "XDR", 31.219M); // MMF - ZPČ (1)
            TestValue(currencies, "NOK", 2.634M); // Norsko - koruna (1)
            TestValue(currencies, "NZD", 14.989M); // Nový Zéland - dolar (1)
            TestValue(currencies, "PLN", 5.978M); // Polsko - zlotý (1)
            TestValue(currencies, "RON", 5.396M); // Rumunsko - leu (1)
            TestValue(currencies, "RUB", 0.35567M); // Rusko - rubl (100)
            TestValue(currencies, "SGD", 16.563M); // Singapur - dolar (1)
            TestValue(currencies, "SEK", 2.416M); // Švédsko - koruna (1)
            TestValue(currencies, "CHF", 22.941M); // Švýcarsko - frank (1)
            TestValue(currencies, "THB", 0.72932M); // Thajsko - baht (100)
            TestValue(currencies, "TRY", 3.894M); // Turecko - lira (1)
            TestValue(currencies, "USD", 22.434M); // USA - dolar (1)
            TestValue(currencies, "GBP", 28.444M); // Velká Británie - libra (1)
        }

        private static void TestValue(Dictionary<string, decimal> currencies, string code, decimal expected)
        {
            bool exists = currencies.ContainsKey(code);
            Assert.IsTrue(exists, $"Currency {code} should be in output, but is missing");
            if (exists)
            {
                decimal value = currencies[code];
                Assert.AreEqual(expected, value, $"Exchange rate for currency {code} differs than expected value");
            }
        }
    }
}
