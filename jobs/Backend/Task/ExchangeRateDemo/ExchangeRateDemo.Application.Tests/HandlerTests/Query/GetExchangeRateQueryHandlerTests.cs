using ExchangeRateDemo.Application.Handlers.Queries.GetExchangeRates;
using ExchangeRateDemo.Application.Handlers.Queries.GetExchangeRates.Models;
using ExchangeRateDemo.Application.Tests.HandlerTests.Base;
using Moq;
using Xunit;

namespace ExchangeRateDemo.Application.Tests.HandlerTests.Query
{
    public class GetExchangeRateQueryHandlerTests : HandlerTest<GetExchangeRatesQueryHandler, GetExchangeRatesQuery, IEnumerable<GetExchangeRatesResponse>>
    {
        protected override GetExchangeRatesQueryHandler Handler => new(ExchangeRateProvider.Object);

        [Theory]
        [MemberData(nameof(Data))]
        public async Task GetExchangeRates(List<string> isoCodes, int expectedRatesCount)
        {
            //Arrange
            var request = new GetExchangeRatesQuery(isoCodes: isoCodes);
            var queryResponse = CreateDefaultRates();

            ExchangeRateProvider.Setup(erp => erp.GetExchangeRates(It.IsAny<string>()))
                .ReturnsAsync(queryResponse);

            //Act
            var response = await Handle(request);

            //Assert
            Assert.Equal(response.Count(), expectedRatesCount);
        }

        public static List<ExchangeRate> CreateDefaultRates() =>
        [
            new()
            {
                CurrencyCode = "AA",
                Country = "A_Country",
                Currency = "A_Currency",
                Amount = 1,
                Rate = 1.009m
            },
            new()
            {
                CurrencyCode = "BB",
                Country = "B_Country",
                Currency = "B_Currency",
                Amount = 1,
                Rate = 2.009m
            },
            new()
            {
                CurrencyCode = "CC",
                Country = "C_Country",
                Currency = "C_Currency",
                Amount = 1,
                Rate = 3.009m
            },
            new()
            {
                CurrencyCode = "DD",
                Country = "D_Country",
                Currency = "D_Currency",
                Amount = 1,
                Rate = 4.009m
            },
            new()
            {
                CurrencyCode = "EE",
                Country = "E_Country",
                Currency = "E_Currency",
                Amount = 1,
                Rate = 5.009m
            }
        ];

        public static TheoryData<List<string>, int> Data
        {
            get
            {
                return new TheoryData<List<string>, int>
                {
                    { null, 5 },
                    { new List<string> { "" }, 5 },
                    { new List<string> { "ZZ" }, 0 },
                    { new List<string> { "AA", "CC" }, 2 }
                };
            }
        }
    }
}
