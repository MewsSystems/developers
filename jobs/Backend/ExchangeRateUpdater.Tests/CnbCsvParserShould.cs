using ExchangeRateUpdated.Service.Parsers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace ExchangeRateUpdater.Tests
{
    [TestClass]
    public class CnbCsvParserShould
    {
        [TestMethod]
        public void ParseSample()
        {
            var cnbCsvParser = new CnbCsvParser();

            var sample = File.Open("./Samples/17June.txt", FileMode.Open);

            var result = cnbCsvParser.ParseExchangeRates(sample);

            var resultList = result.ToList();

            result.Count().Should().Be(32);
        }
    }
}