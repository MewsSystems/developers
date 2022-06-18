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
        public void ParseSample_With_Valid_data_ReturnsOk()
        {
            var cnbCsvParser = new CnbCsvParser();

            var sample = File.Open("./Samples/17June.txt", FileMode.Open, FileAccess.Read);

            var result = cnbCsvParser.TryParseExchangeRates(sample);

            result.IsSuccess.Should().BeTrue();
            result.Value.Count().Should().Be(32);
        }

        [TestMethod]
        public void ParseSample_WithClosedStream_Returns_Failure()
        {
            var cnbCsvParser = new CnbCsvParser();

            var streamWriter = new MemoryStream();
            streamWriter.Close(); // intentionally close stream

            var result = cnbCsvParser.TryParseExchangeRates(streamWriter);

            result.IsFailed.Should().BeTrue();
        }

        [TestMethod]
        public void ParseSample_With_InvalidData_Returns_Failure()
        {
            var cnbCsvParser = new CnbCsvParser();

            var sample = File.Open("./Samples/InvalidData.txt", FileMode.Open, FileAccess.Read);

            var result = cnbCsvParser.TryParseExchangeRates(sample);

            result.IsFailed.Should().BeTrue();
        }
    }
}