using ExchangeRateUpdater.CoreClasses;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Test.CoreClasses
{
    [TestFixture]
    public class ExchangeRate_tests
    {
        [Test]
        public void Constructor_Exceptions_tests()
        {
            var cSource = new Currency("RUB");
            var cTarget = new Currency("CZK");

            Assert.Throws<ArgumentNullException>(() => new ExchangeRate(null, 1, cTarget, 1, 1));
            Assert.Throws<ArgumentNullException>(() => new ExchangeRate(cSource, 1, null, 1, 1));
            Assert.Throws<ArgumentException>(() => new ExchangeRate(cSource, 1, cSource, 1, 1));
        }

        [Test]
        public void ToString_tests()
        {
            var cSoruceCode = "RUB";
            var cTargetCode = "CZK";
            var amountSource = 100;
            var amountTarget = 1;
            var quote = 99.99m;
            var cSource = new Currency(cSoruceCode);
            var cTarget = new Currency(cTargetCode);
            var exchangeRate = new ExchangeRate(cSource, amountSource, cTarget, amountTarget, quote).ToString();

            var expected = $"{amountSource} {cSoruceCode} / {amountTarget} {cTargetCode} = {quote}";


            Assert.AreEqual(expected, exchangeRate);
        }
    }
}
