using System;
using System.Globalization;
using System.Threading;
using ExchangeRateUpdater.Cnb;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeRateUpdater.Tests.Cnb
{
    [TestClass]
    public class ExchangeRateParserTests
    {
        [TestMethod]
        public void Parses_correct_multiple_exchange_rates()
        {
            var data = @"19 Nov 2019 #224
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|15.748
Brazil|real|1|BRL|5.503
";

            var parser = new ExchangeRateParser();
            var rates = parser.Parse(data, new Currency("CZK"));

            Assert.AreEqual(2, rates.Length);
            Assert.AreEqual("CZK", rates[0].TargetCurrency.Code);
            Assert.AreEqual("AUD", rates[0].SourceCurrency.Code);
            Assert.AreEqual(15.748M, rates[0].Value);
            Assert.AreEqual("CZK", rates[1].TargetCurrency.Code);
            Assert.AreEqual("BRL", rates[1].SourceCurrency.Code);
            Assert.AreEqual(5.503M, rates[1].Value);
        }

        [TestMethod]
        public void Parses_exchange_rate_with_amount_other_than_1()
        {
            var data = @"19 Nov 2019 #224
Country|Currency|Amount|Code|Rate
Hungary|forint|100|HUF|7.633
";

            var parser = new ExchangeRateParser();
            var rates = parser.Parse(data, new Currency("CZK"));

            Assert.AreEqual(1, rates.Length);
            Assert.AreEqual(0.07633M, rates[0].Value);
        }

        [TestMethod]
        public void Recognizes_both_line_endings()
        {
            var data = "19 Nov 2019 #224\nCountry|Currency|Amount|Code|Rate\r\nHungary|forint|100|HUF|7.633";

            var parser = new ExchangeRateParser();
            var rates = parser.Parse(data, new Currency("CZK"));

            Assert.AreEqual(1, rates.Length);
        }

        [TestMethod]
        public void Can_parse_prices_When_current_culture_is_nonUS()
        {
            var orignalCulture = Thread.CurrentThread.CurrentCulture;
            var orignalUICulture = Thread.CurrentThread.CurrentUICulture;

            try
            {
                var ci = new CultureInfo("cs-CZ");
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;

                var data = "19 Nov 2019 #224\nCountry|Currency|Amount|Code|Rate\r\nHungary|forint|1|HUF|7.633";

                var parser = new ExchangeRateParser();
                var rates = parser.Parse(data, new Currency("CZK"));

                Assert.AreEqual(1, rates.Length);
                Assert.AreEqual(7.633M, rates[0].Value);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = orignalCulture;
                Thread.CurrentThread.CurrentUICulture = orignalUICulture;
            }
        }

        [TestMethod]
        public void Throws_When_unexpected_header()
        {
            var data = "19 Nov 2019 #224\nblabla|Currency|blabla|Code|Rate\r\nHungary|forint|100|HUF|7.633";

            var parser = new ExchangeRateParser();
            Assert.ThrowsException<ExchangeRateProviderException>(() => _ = parser.Parse(data, new Currency("CZK")));
        }

        [TestMethod]
        public void Throws_When_header_missing()
        {
            var data = "19 Nov 2019 #224";

            var parser = new ExchangeRateParser();
            Assert.ThrowsException<ExchangeRateProviderException>(() => _ = parser.Parse(data, new Currency("CZK")));
        }

        [TestMethod]
        public void Throws_When_empty_data()
        {
            var data = "";

            var parser = new ExchangeRateParser();
            Assert.ThrowsException<ExchangeRateProviderException>(() => _ = parser.Parse(data, new Currency("CZK")));
        }

        [TestMethod]
        public void Throws_When_not_enough_exchange_rate_values()
        {
            var data = "19 Nov 2019 #224\nblabla|Currency|blabla|Code|Rate\r\nHungary|HUF|7.633";

            var parser = new ExchangeRateParser();
            Assert.ThrowsException<ExchangeRateProviderException>(() => _ = parser.Parse(data, new Currency("CZK")));
        }

        [TestMethod]
        public void Throws_When_invalid_amount()
        {
            var data = "19 Nov 2019 #224\nblabla|Currency|blabla|Code|Rate\r\nHungary|forint|invalid|HUF|7.633";

            var parser = new ExchangeRateParser();
            Assert.ThrowsException<ExchangeRateProviderException>(() => _ = parser.Parse(data, new Currency("CZK")));
        }

        [TestMethod]
        public void Throws_When_invalid_value()
        {
            var data = "19 Nov 2019 #224\nblabla|Currency|blabla|Code|Rate\r\nHungary|forint|100|HUF|invalid";

            var parser = new ExchangeRateParser();
            Assert.ThrowsException<ExchangeRateProviderException>(() => _ = parser.Parse(data, new Currency("CZK")));
        }

        [TestMethod]
        public void Throws_When_invalid_currency_code()
        {
            var data = "19 Nov 2019 #224\nblabla|Currency|blabla|Code|Rate\r\nHungary|forint|100|invalid|7.633";

            var parser = new ExchangeRateParser();
            Assert.ThrowsException<ExchangeRateProviderException>(() => _ = parser.Parse(data, new Currency("CZK")));
        }
    }
}
