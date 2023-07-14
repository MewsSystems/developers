using ExchangeRateUpdater.Interface.DTOs;
using ExchangeRateUpdater.Interface.Model.Validators;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeRateUpdater.Test.Validators.Dtos
{
    [TestClass]
    public class CurrencyDtoValidatorTest
    {
        private readonly IValidator<CurrencyDto> _validator;

        public CurrencyDtoValidatorTest()
        {
            _validator = new CurrencyDtoValidator();
        }

        [TestMethod]
        public void IsValid()
        {
            var currency = new CurrencyDto { Code = "USD" };

            var result = _validator.Validate(currency);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void IsNotValid_Currency_Code_Is_Empty()
        {
            var currency = new CurrencyDto { Code = string.Empty };

            var result = _validator.Validate(currency);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        [DataRow("AA")]
        [DataRow("AAAA")]
        public void IsNotValid_Currency_Code_Lenght_Is_Not_3(string code)
        {
            var currency = new CurrencyDto { Code = code };

            var result = _validator.Validate(currency);

            Assert.IsFalse(result.IsValid);
        }
    }
}
