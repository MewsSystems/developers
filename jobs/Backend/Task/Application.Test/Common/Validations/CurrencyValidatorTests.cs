using Application.Common.Models;
using Application.Common.Validations;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Test.Common.Validations
{
    public class CurrencyValidatorTests
    {
        private readonly CurrencyValidator currencyValidator;

        public CurrencyValidatorTests()
        {
            currencyValidator = new CurrencyValidator();
        }

        [Fact]
        public void CurrencyValidator_WithValidCurrencyCode_Passes()
        {
            var currencyCode = new Currency("CZK");

            var result = currencyValidator.Validate(currencyCode);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void CurrencyValidator_WithInvalidCurrencyCode_Fails()
        {
            var currencyCode = new Currency("XYZ");

            var result = currencyValidator.Validate(currencyCode);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void CurrencyValidator_WithBlankCurrencyCode_Fails()
        {
            var currencyCode = new Currency("");

            var result = currencyValidator.Validate(currencyCode);

            result.IsValid.Should().BeFalse();
        }
    }
}
