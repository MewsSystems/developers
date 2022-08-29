using CsvHelper;
using CsvHelper.Configuration;
using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Parsers;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unit.Tests
{
    //The parser was made using CSV Helper library, really no point testing in depth 
    public class CNBExchangeRateParserTests
    {
        private Mock<ICzechNationalBankConfiguration> cnbConfig;
      
        readonly string validData = "29 Aug 2022 #167\n\rCountry|Currency|Amount|Code|Rate\n\rAustralia|dollar|1|AUD|16.927";

        [SetUp]
        public void Setup()
        {
            cnbConfig = new Mock<ICzechNationalBankConfiguration>();
            cnbConfig.Setup(x => x.CSVResponseDelimiter()).Returns("|");
            cnbConfig.Setup(x => x.CSVExpectingHeaders()).Returns(true);
        }

        [Test]
        public void Should_Parse_CNBResponse()
        {
            var parser = new CNBExchangeRateParser(cnbConfig.Object);
             
            var result = parser.ParseCNBResponse(GenerateStreamFromString(validData));

            Assert.That(result, Is.Not.Null);
        }

        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}