using ExchangeRateUpdater;
using System;
using Xunit;

namespace ExchangeRateUpdate.UnitTests.Parsers
{
    public class TextCnbParserTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void WontParse_WhenNullArgument_ThrowsException(string content)
        {
            var parser = new TextCnbParser();

            Assert.Throws<ArgumentException>(() => parser.TryParse(content, out var res));
        }

        [Theory]
        [InlineData("ABC")]
        [InlineData("Austrálie|dolar|1|AUD|TEST|15,694")]
        public void WontParse_WhenInvalidContent_ReturnFalse(string content)
        {
            var parser = new TextCnbParser();

            Assert.False(parser.TryParse(content, out var res));
        }

        [Theory]
        [InlineData("Austrálie|dolar|1|AUD|15,694\nBrazílie|real|1|BRL|4,072\nBulharsko|lev|1|BGN|12,988")]
        [InlineData("Austrálie|dolar|1|AUD|15,694")]
        [InlineData("21.09.2021 #183\nAustrálie|dolar|1|AUD|15,694")]
        [InlineData("21.09.2021 #183\nzemě|měna|množství|kód|kurz\nAustrálie|dolar|1|AUD|15,694")]
        [InlineData("země|měna|množství|kód|kurz\nAustrálie|dolar|1|AUD|15,694")]
        public void WillParse_WhenValidContent_ReturnTrue(string content)
        {
            var parser = new TextCnbParser();

            Assert.True(parser.TryParse(content, out var res));
            Assert.NotNull(res);
        }
    }
}
