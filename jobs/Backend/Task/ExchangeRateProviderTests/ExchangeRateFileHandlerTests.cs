using ExchangeRateProvider;
using NUnit.Framework;
using System;
using System.IO;

namespace ExchangeRateProviderTests
{
    [TestFixture]
    public class ExchangeRateFileHandlerTests
    {
        [Test]
        public void Read_InvalidPath_ExceptionIsThrown()
        {
            // Arange
            ExchangeRateFileHandler fileHandler = new ExchangeRateFileHandler();
            fileHandler.CachedFilePath = "dummy path";

            // Act - Assert
            var ex = Assert.Throws<FileNotFoundException>(() => fileHandler.Read());
            Assert.That(ex.Message, Is.EqualTo("Unable to find the specified file."));
        }

        [Test]
        public void Read_InvalidFile_ExceptionIsThrown()
        {
            // Arange
            string path = Path.Combine(Environment.CurrentDirectory, "ReadInvalidFile.txt");
            ExchangeRateFileHandler fileHandler = new ExchangeRateFileHandler();
            fileHandler.CachedFilePath = path;
            using (var sw = new StreamWriter(path))
            {
                sw.WriteLine("1\"|NOK|CZK|2.572");
                sw.WriteLine("100|PHP|CZK|43.865");
            }

            // Act - Assert
            var ex = Assert.Throws<FormatException>(() => fileHandler.Read());
            Assert.That(ex.Message, Is.EqualTo("Input string was not in a correct format."));
        }

        [Test]
        public void Read_ValidPath_ReturnsExpectedExchangeRates()
        {
            // Arange
            string path = Path.Combine(Environment.CurrentDirectory, "ReadValidFile.txt");
            ExchangeRateFileHandler fileHandler = new ExchangeRateFileHandler();
            fileHandler.CachedFilePath = path;
            using (var sw = new StreamWriter(path))
            {
                sw.WriteLine("1|NOK|CZK|2.572");
            }

            CurrencyCode code = new CurrencyCode("NOK");
            ExchangeRate exchangeRate = new ExchangeRate(1, code, new CurrencyCode("CZK"), new decimal(2.572));

            // Act
            var actuall = fileHandler.Read();

            // Assert
            Assert.IsTrue(actuall[code].Equals(exchangeRate));
        }
    }
}
