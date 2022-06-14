
using Xunit;

namespace Task.UnitTests.Services
{




    public class ExchangeRateProviderHelperTests
    {


        [Theory]
        [InlineData("EUR")]
        [InlineData("USD")]
        [InlineData("HUF")]
        [InlineData("CNY")]
        [InlineData("PLN")]
        public void CorrectInputReturnCorrectOutcome(string currency)
        {
            var exchangeRateValidationsHelper = new ExchangeRateValidationsHelper();
            var settings = new Settings()
            {
                SupportedCurrencies = new List<string>() { "EUR", "USD", "HUF", "CNY", "PLN" },
            };
            var result = exchangeRateValidationsHelper.ValidateCurrency(currency, settings);

            Assert.True(result);
        }

        [Theory]
        [InlineData("LSD")]
        [InlineData("FDW")]
        [InlineData("WQS")]
        [InlineData("DSX")]
        [InlineData("QQQ")]
        public void WrongCurrenciesCorrectFormatReturnFalseAndPrintsCorrectMessage(string currency)
        {
            var exchangeRateValidationsHelper = new ExchangeRateValidationsHelper();
            var settings = new Settings()
            {
                SupportedCurrencies = new List<string>() { "EUR", "USD", "HUF", "CNY", "PLN" },
            };

            var output = new StringWriter();
            Console.SetOut(output);
            var result = exchangeRateValidationsHelper.ValidateCurrency(currency, settings);

            var outputString = output.ToString();
            Assert.Equal($"Currency {currency} is not supported/does not exist\n", outputString);
            Assert.False(result);
        }

        [Theory]
        [InlineData("112")]
        [InlineData("%$#")]
        [InlineData("1@e")]
        [InlineData("12S")]
        [InlineData("%%F")]
        public void WrongCurrenciesWrongFormatReturnFalseAndPrintsCorrectMessage(string currency)
        {
            var exchangeRateValidationsHelper = new ExchangeRateValidationsHelper();
            var settings = new Settings()
            {
                SupportedCurrencies = new List<string>() { "EUR", "USD", "HUF", "CNY", "PLN" },
            };

            var output = new StringWriter();
            Console.SetOut(output);
            var result = exchangeRateValidationsHelper.ValidateCurrency(currency, settings);

            var outputString = output.ToString();
            Assert.Equal("Currency should consist only of letters\n", outputString);
            Assert.False(result);
        }


        [InlineData("1121")]
        [InlineData("%#")]
        [InlineData("ZX")]
        [InlineData("XCZS")]
        [InlineData("F")]
        public void WrongCurrenciesWrongSizeReturnFalseAndPrintsCorrectMessage(string currency)
        {
            var exchangeRateValidationsHelper = new ExchangeRateValidationsHelper();
            var settings = new Settings()
            {
                SupportedCurrencies = new List<string>() { "EUR", "USD", "HUF", "CNY", "PLN" },
            };

            var output = new StringWriter();
            Console.SetOut(output);
            var result = exchangeRateValidationsHelper.ValidateCurrency(currency, settings);

            var outputString = output.ToString();
            Assert.Equal("Currency codes should be 3 characters (ISO 4217)\n", outputString);
            Assert.False(result);
        }

    }
}