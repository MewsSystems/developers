using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.Application
{
    /// <summary>
    /// Implementation of <see cref="IExchangeRateProvider"/> using CNB rates related rooted to CZK.
    /// 
    /// TODO: Could be moved to domain now, but nobody knows if some CNB specific changes will be done here in future.
    /// </summary>
    public class CnbExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IExchangeRateRepository _exchangeRateRepository;

        public CnbExchangeRateProvider(IExchangeRateRepository exchangeRateRepository)
        {
            _exchangeRateRepository = exchangeRateRepository;
        }

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (!_exchangeRateRepository.Any())
            { 
                return Enumerable.Empty<ExchangeRate>();
            }

            List<ExchangeRate> rates = new List<ExchangeRate>();

            foreach (var currency in currencies)
            {
                var rate = _exchangeRateRepository.TryGet(currency.Code);

                if (rate != null)
                {
                    rates.Add(rate);
                }
            }

            return rates;
        }
    }
}