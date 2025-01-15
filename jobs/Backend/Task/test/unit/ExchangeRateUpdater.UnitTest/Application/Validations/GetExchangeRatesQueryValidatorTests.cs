using ExchangeRateUpdater.Application.Queries.ExchangeRates.GetExchangeRates;
using FluentAssertions;

namespace ExchangeRateUpdater.UnitTest.Application.Validations
{
    public class GetExchangeRatesQueryValidatorTests
    {
        private readonly GetExchangeRatesQueryValidator _validator;

        public GetExchangeRatesQueryValidatorTests()
        {
            _validator = new GetExchangeRatesQueryValidator();
        }

        [Theory, MemberData(nameof(DatesToTest))]
        public async Task Validate_Date_Works(DateTime? date, bool isValid)
        {
            // Arrange
            var query = new GetExchangeRatesQuery
            {
               Currencies = ["AUD"],
               Date = date
            };

            // Act
            var result = await _validator.ValidateAsync(query);

            // Assert
            result.IsValid.Should().Be(isValid);
        }

        public static TheoryData<DateTime?, bool> DatesToTest =>
            new()
            {
                { null, true },
                { DateTime.Today.AddDays(-1), true },
                { DateTime.Today, true },
                { DateTime.Today.AddDays(1), false }
            };
    }
}
