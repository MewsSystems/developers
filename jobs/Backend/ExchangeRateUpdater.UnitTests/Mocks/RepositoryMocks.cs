using ExchangeRateUpdater.Persistence;
using ExchangeRateUpdater.Models.Types;
using Moq;

namespace ExchangeRateUpdater.UnitTests.Mocks
{
    internal static class RepositoryMocks
    {
        internal static Mock<IExchangeRateRepository> GetExchangeRateRepository()
        {
            var currencies = new List<Currency>
            {
                new Currency(new Code("USD")),
                new Currency(new Code("EUR")),
                new Currency(new Code("CZK")),
                new Currency(new Code("JPY")),
                new Currency(new Code("KES")),
                new Currency(new Code("RUB")),
                new Currency(new Code("THB")),
                new Currency(new Code("TRY")),
                new Currency(new Code("XYZ"))
            };

            var exchangeRateRepository = new Mock<IExchangeRateRepository>();
            exchangeRateRepository.Setup(x => x.GetSourceCurrencies()).Returns(currencies);

            return exchangeRateRepository;
        }

    }
}