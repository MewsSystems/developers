using Application.Abstractions;
using Domain.Entities;

namespace Application.ExchangeRateProvider
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {

        private readonly IExchangeRateRepository _exchangeRateRepository;

        public ExchangeRateProvider(IExchangeRateRepository exchangeRateRepository)
        {
            _exchangeRateRepository = exchangeRateRepository;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {

            if (currencies is null || !currencies.Any())
                return Enumerable.Empty<ExchangeRate>();

            IDictionary<string, ExchangeRate> exchangeRateRepository = await _exchangeRateRepository.GetTodayCZKExchangeRatesDictionaryAsync();

            if (exchangeRateRepository is null || !exchangeRateRepository.Any())
                return Enumerable.Empty<ExchangeRate>();

            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();    

            foreach (Currency currency in currencies)
            {

                if (exchangeRateRepository.TryGetValue(currency.Code, out ExchangeRate exchangeRate))
                    exchangeRates.Add(exchangeRate);

            }

            return exchangeRates;

        }
    }
}
