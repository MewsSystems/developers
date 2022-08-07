using CsvHelper;
using ExchangeRateUpdater.ExchangeRateParsers;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace ExchangeRateUpdaterTest
{
    public class CnbExchangeRateParserTests
    {        
        [Fact]
        public void ParceBadData_HeaderValidationException()
        {
            var parser = new CnbExchangeRateParser();
            var fs = new FileStream("TestData/BadData.txt", FileMode.Open);
            Assert.Throws<HeaderValidationException>(() => parser.Parse(fs));            
        }

        [Fact]
        public void ParceEmptyData_Success()
        {
            var parser = new CnbExchangeRateParser();
            var emptyStream = new MemoryStream();
            var result = parser.Parse(emptyStream);

            Assert.NotNull(result);
            Assert.False(result.Any());
        }

        [Fact]        
        public void ParceNullData_ArgumentNullException()
        {
            var parser = new CnbExchangeRateParser();            
            Assert.Throws<ArgumentNullException>(() => parser.Parse(null));
        }
    }
}