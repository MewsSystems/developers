using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ExchangeRateUpdater.Cnb
{
    public class Parser
    {
        [Fact]
        public void LineNull_RateEmpty()
        {
            //Arrange
            string[] data = null;

            //Act
            ExchangeRate rate = CnbDataParser
                .ParseLine(data);

            //Assert
            Assert.NotNull(rate);
            Assert.NotNull(rate.TargetCurrency);
            Assert.Null(rate.TargetCurrency.Code);
            Assert.NotNull(rate.SourceCurrency);
            Assert.Equal("CZK", rate.SourceCurrency.Code);
            Assert.Equal(0.0m, rate.Value);
        }

        [Fact]
        public void LineEmpty_RateEmpty()
        {
            //Arrange
            var data = new string[0];

            //Act
            ExchangeRate rate = CnbDataParser
                .ParseLine(data);

            //Assert
            Assert.NotNull(rate);
            Assert.NotNull(rate.TargetCurrency);
            Assert.Null(rate.TargetCurrency.Code);
            Assert.NotNull(rate.SourceCurrency);
            Assert.Equal("CZK", rate.SourceCurrency.Code);
            Assert.Equal(0.0m, rate.Value);
        }

        [Fact]
        public void DataNull_RatesEmpty()
        {
            //Arrange
            string data = null;

            //Act
            IEnumerable<ExchangeRate> rates = new CnbDataParser()
                .ParseData(data);

            //Assert
            Assert.Empty(rates);
        }

        [Fact]
        public void DataEmpty_RatesEmpty()
        {
            //Arrange
            string data = string.Empty;

            //Act
            IEnumerable<ExchangeRate> rates = new CnbDataParser()
                .ParseData(data);

            //Assert
            Assert.Empty(rates);
        }

        [Fact]
        public void InvalidData_EmptyRates()
        {
            //Arrange
            var data = "test\ntest2\ntest3|add\n";

            //Act
            IEnumerable<ExchangeRate> rates = new CnbDataParser()
                .ParseData(data);

            //Assert
            Assert.Empty(rates);
        }

        [Fact]
        public void NoRates_EmptyRates()
        {
            //Arrange
            var data = "header\nzemě|měna|množství|kód|kurz";

            //Act
            IEnumerable<ExchangeRate> rates = new CnbDataParser()
                .ParseData(data);

            //Assert
            Assert.Empty(rates);
        }

        [Fact]
        public void SomeData_Success()
        {
            //Arrange
            var data =
                "21.02.2020 #37\nzemě|měna|množství|kód|kurz\nAustrálie|dolar|1|AUD|15,319\nBrazílie|real|1|BRL|5,272";

            //Act
            IEnumerable<ExchangeRate> rates = new CnbDataParser()
                .ParseData(data);

            //Assert
            Assert.Equal(2, rates.Count());

            Assert.Equal("CZK", rates.First().SourceCurrency.Code);
            Assert.Equal("AUD", rates.First().TargetCurrency.Code);
            Assert.Equal(15.319m, rates.First().Value);

            Assert.Equal("CZK", rates.Skip(1).First().SourceCurrency.Code);
            Assert.Equal("BRL", rates.Skip(1).First().TargetCurrency.Code);
            Assert.Equal(5.272m, rates.Skip(1).First().Value);
        }
    }
}