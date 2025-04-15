using ExchangeRateUpdater.Domain.DTOs;
using ExchangeRateUpdater.Domain.Validators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace ExchangeRateUpdater.Tests.Domain;

[TestFixture]
    public class CnbExchangeRateValidatorTests
    {
        private CnbExchangeRateValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new CnbExchangeRateValidator();
        }

        [Test]
        public void Should_HaveError_When_CurrencyCodeIsEmpty()
        {
            var dto = new CnbExchangeRateDto
            {
                CurrencyCode = "",
                ValidFor = DateTime.Today.ToString("yyyy-MM-dd"),
                Amount = 1,
                Rate = 10m,
                Country = "SomeCountry"
            };
            
            var result = _validator.TestValidate(dto);
            
            result.ShouldHaveValidationErrorFor(x => x.CurrencyCode);
        }

        [Test]
        public void Should_HaveError_When_ValidForIsInFuture()
        {
            var futureDate = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
            var dto = new CnbExchangeRateDto
            {
                CurrencyCode = "USD",
                ValidFor = futureDate,
                Amount = 1,
                Rate = 10m,
                Country = "USA"
            };
            
            var result = _validator.TestValidate(dto);
            
            result.ShouldHaveAnyValidationError();
        }

        [Test]
        public void Should_HaveNoErrors_When_ValidForIsToday()
        {
            var dto = new CnbExchangeRateDto
            {
                CurrencyCode = "EUR",
                ValidFor = DateTime.Today.ToString("yyyy-MM-dd"),
                Amount = 1,
                Rate = 25.1m,
                Country = "EMU"
            };
            
            var result = _validator.TestValidate(dto);
            
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_HaveError_When_OlderThan5Days()
        {
            var oldDate = DateTime.Today.AddDays(-6).ToString("yyyy-MM-dd");
            var dto = new CnbExchangeRateDto
            {
                CurrencyCode = "EUR",
                ValidFor = oldDate,
                Amount = 1,
                Rate = 25.1m,
                Country = "EMU"
            };
            
            var result = _validator.TestValidate(dto);
            
            result.ShouldHaveAnyValidationError();
        }
    }