using Application.Abstractions;
using Domain.Entities;
using NSubstitute;

namespace Application.Test
{
    public class ExchangeRateProviderTests
    {


        private readonly IExchangeRateRepository _exchangeRateRepositoryMock;

        private readonly Dictionary<string, ExchangeRate> _exchangeRateRepositoryData = new Dictionary<string, ExchangeRate>()
        {
            { "EUR", ExchangeRate.Create("EUR", "CZD", (decimal) 0.88) },
            { "USD", ExchangeRate.Create("USD", "CZD", (decimal) 0.68) },
            { "PAR", ExchangeRate.Create("PAR", "CZD", (decimal) 0.48) },
        };

        public ExchangeRateProviderTests() 
        {

            _exchangeRateRepositoryMock = Substitute.For<IExchangeRateRepository>();
            _exchangeRateRepositoryMock.GetTodayCZKExchangeRatesDictionaryAsync()
                .Returns(Task.FromResult<IDictionary<string, ExchangeRate>>(
                    _exchangeRateRepositoryData
                ));
        }

        [Fact]
        public async Task ExchangeRateProvider_GetExchangeRates_CurrenciesIsNull()
        {


            var actor = new ExchangeRateProvider.ExchangeRateProvider(_exchangeRateRepositoryMock);

            var action = await actor.GetExchangeRates(null);

            Assert.True(!action.Any());

        }

        [Fact]
        public async Task ExchangeRateProvider_GetExchangeRates_CurrenciesIsEmpty()
        {


            var actor = new ExchangeRateProvider.ExchangeRateProvider(_exchangeRateRepositoryMock);

            var action = await actor.GetExchangeRates(new List<Currency>());

            Assert.True(!action.Any());

        }

        [Fact]
        public async Task ExchangeRateProvider_GetExchangeRates_ValidInput()
        {


            var actor = new ExchangeRateProvider.ExchangeRateProvider(_exchangeRateRepositoryMock);

            var input = new List<Currency>()
            {
                new Currency("EUR"),
                new Currency("USD"),
                new Currency("NOE")
            };

            var action = await actor.GetExchangeRates(input);

            Assert.True(action.Count() == 2);
            Assert.True(action.First(i => i.SourceCurrency.Code == "EUR").Value.Value == (decimal)0.88);
            Assert.True(action.First(i => i.SourceCurrency.Code == "USD").Value.Value == (decimal)0.68);
        }

    }
}
