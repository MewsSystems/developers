using ExchangeRateUpdater.ExchangeProviders;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExchangeRateUpdater.Tests.ExchangeProviders
{
    [TestClass]
    public class CNBParserUnitTests
    {
        private ICNBParser CNBParser;

        public CNBParserUnitTests()
        {
            var logger = new Mock<ILogger<CNBParser>>();
            CNBParser = new CNBParser(logger.Object);
        }

        [TestMethod]
        public void Parse_ValidData_ReturnsValidData()
        {
            // Arrange
            var sb = new StringBuilder();
            sb.AppendLine("14.02.2022 #31");
            sb.AppendLine("země|měna|množství|kód|kurz");
            sb.AppendLine("Austrálie|dolar|1|AUD|15,428");
            sb.AppendLine("Brazílie|real|1|BRL|4,161");
            sb.AppendLine("USA|dolar|1|USD|21,676");

            var currencies = new List<Currency>() { new Currency("USD"), new Currency("EUR") };

            // Act
            var result = CNBParser.Parse(sb.ToString(), currencies);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public void Parse_Empty_ReturnsEmpty()
        {
            // Arrange
            var data = "";
            var currencies = Enumerable.Empty<Currency>();

            // Act
            var result = CNBParser.Parse(data, currencies);

            //Assert
            Assert.IsTrue(result.IsFailed);
        }

        [TestMethod]
        public void Parse_InvalidData_ReturnsEmpty()
        {
            // Arrange
            var data = "IAM_INVALID\r\nDATA!!";
            var currencies = Enumerable.Empty<Currency>();

            // Act
            var result = CNBParser.Parse(data, currencies);

            // Assert
            Assert.IsTrue(result.IsFailed);
        }
    }
}