using ExchangeRateUpdater.Implementation.Queries;
using ExchangeRateUpdater.Interface.DTOs;
using FluentValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Test.Validators.Queries
{
    [TestClass]
    public class GetExchangeRatesQueryValidatorTest
    {
        private readonly IValidator<GetExchangeRatesQuery> _validator;

        public GetExchangeRatesQueryValidatorTest()
        {
            _validator = new GetExchangeRatesQueryValidator();
        }

        [TestMethod]
        public void IsValid()
        {
            var query = new GetExchangeRatesQuery 
            { 
                Currencies = new List<CurrencyDto> 
                { 
                    new CurrencyDto
                    {
                        Code = "USD"
                    }
                } 
            };

            var result = _validator.Validate(query);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void IsNotValid_Query_Is_Null()
        {
            var query = new GetExchangeRatesQuery();

            var result = _validator.Validate(query);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void IsNotValid_Query_Is_Empty()
        {
            var query = new GetExchangeRatesQuery { Currencies = new List<CurrencyDto>() };

            var result = _validator.Validate(query);

            Assert.IsFalse(result.IsValid);
        }
    }
}
